using Jeon.CommonFramework.Win32;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonFramework
{
    /// <summary>
    /// Windows API
    /// </summary>
    internal class WinAPI
    {
        #region Document ------------------------------------------------------
        /*
         * 작성자: jeon
         * 설명: Windows API를 모두 이곳에 모아둔다.
         */
        #endregion Document ---------------------------------------------------

        #region kernel32.dll --------------------------------------------------

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string IpModuleName);


        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        [DllImport("kernel32", EntryPoint = "GetLastError")]
        public extern static int __GetLastError();
        [DllImport("Kernel32.dll", EntryPoint = "GlobalMemoryStatusEx", SetLastError = true)]
        public extern static bool __GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        public static MEMORYSTATUSEX GlobalMemoryStatusEx()
        {
            MEMORYSTATUSEX memstat = new MEMORYSTATUSEX();

            memstat.dwLength = (uint)Marshal.SizeOf(memstat);
            if (__GlobalMemoryStatusEx(ref memstat) == false)
            {
                int error = __GetLastError();
                throw new System.ComponentModel.Win32Exception(error);
            }
            return memstat;
        }
        #endregion kernel32.dll -----------------------------------------------

        /// <summary>
        /// Caret을 숨긴다.
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <returns>성공 여부</returns>
        [DllImport("user32.dll")]
        public static extern bool HideCaret(IntPtr hWnd);

        /// <summary>
        /// Animate Window
        /// 최소 운영 체제: Windows 98, Windows 2000
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="time">애니메이션 시간 일반적으로 200 밀리초가 걸린다.</param>
        /// <param name="flags">효과 Flag</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hWnd, int time, int flags);

        /// <summary>
        /// 특정 키가 눌렸는지 여부를 구한다.
        /// </summary>
        /// <param name="vKey">구할 키 값</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern short GetAsyncKeyState(int vKey);
        /// <summary>
        /// 특정 키가 눌렸는지 여부를 구한다.
        /// </summary>
        /// <param name="keyCode">구할 키</param>
        /// <returns>눌렸는지 여부</returns>
        public static bool IsKeyPressed(int keyCode)
        {
            return (GetAsyncKeyState(keyCode) & 0x0800) == 0;
        }
        /// <summary>
        /// Desktop의 Handle을 구한다.
        /// </summary>
        /// <returns>Desktop의 Handle</returns>
        [DllImport("user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetDestopWindow();
        /// <summary>
        /// 윈도우의 속성을 구한다.
        /// </summary>
        /// <param name="hWnd">속성을 구하고자 하는 Window handle</param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        /// <summary>
        /// The GetWindowRect function retrieveds the dimensions of the bounding rectangle of the specified window.
        /// The dimensions are given is screen coordinates that are relative to the upper-left corner of the screen.
        /// </summary>
        /// <param name="hWnd">handle to the window</param>
        /// <param name="rect">Pointer to a structur that recevies teh screen coordinates of the upper-left lover-right corners of the window.</param>
        /// <returns>If the function succeeds, the reutnr is nonzero.
        /// If ther function fails, ther return value is zero. To get extended error is information, call GetLastError</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetWindowRect(HandleRef hWnd, [In, Out] ref RECT rect);
        /// <summary>
        /// 윈도우의 속성을 설정한다.
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="nIndex"></param>
        /// <param name="newLong"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int newLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc Ipfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr IParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ReleaseCapture();

        [DllImport("user32")]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern UInt32 RegisterWindowMessage(
            [MarshalAs(UnmanagedType.LPTStr)] String lpString);    // Pointer to a null-terminated string that 


        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        [DllImport("user32.dll")]
        public static extern bool FlashWindowEx(IntPtr pfwi);
        //public static extern bool FlashWindowEx(FlashWindowInfo pfwi);

        /// <summary>
        /// 해당 핸들의 프로세스의 순위를 높여 해당 창의 프로그램을 활성화 합니다.
        /// </summary>
        /// <param name="hWnd"></param>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("shell32.dll", EntryPoint = "ExtractIconA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr ExtractIcon(int hInst, string lpszExeFileName, int nIconIndex);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static bool DestroyIcon(IntPtr handle);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public extern static long ShellExecute(IntPtr hWnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, long nShowCmd);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        /// <summary>
        /// 마지막에 입력된 윈도우 이벤트 발생 시간(TickCount)을 MilliSecond형태로 반환합니다.
        /// </summary>
        /// <param name="lpi">WindowInputInfo</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool GetLastInputInfo(LastInputInfo lpi);

        /// <summary>
        /// 현재 Window에 활성되어 있는 창의 Handle를 가져 옵니다.
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 해당 핸들의 창을 활성 시킵니다.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="cmdShow"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hWnd, short cmdShow);

        /// <summary>
        /// 해당 핸들의 창을 화면 가장 상단으로 이동합니다.
        /// </summary>
        /// <param name="hWnd"></param>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern void BringWindowToTop(IntPtr hWnd);

        /// <SUMMARY>
        /// EnumWindows의 실행 결과를 받아줄 콜백함수이다.
        /// EnumWindows는 이 함수 결과가 false가 될 때까지 계속 윈도우를 검색하게 된다.
        /// </SUMMARY>
        /// <PARAM name="hWnd"></PARAM>
        /// <PARAM name="lParam"></PARAM>
        /// <RETURNS></RETURNS>
        public delegate bool EnumDelegate(IntPtr hWnd, Int32 lParam);
        /// <SUMMARY>
        /// EnumWindows 함수는 모든 최상위 윈도우를 검색해서 그 핸들을 콜백함수로 전달하되
        /// 모든 윈도우를 다 찾거나 콜백함수가 FALSE를 리턴할 때까지 검색을 계속한다.
        /// 콜백함수는 검색된 윈도우의 핸들을 전달받으므로 모든 윈도우에 대해 모든 작업을 다 할 수 있다.
        /// EnumWindows 함수는 차일드 윈도우는 검색에서 제외된다.
        /// 단 시스템이 생성한 일부 최상위 윈도우는 WS_CHILD 스타일을 가지고 있더라도 예외적으로 검색에 포함된다.
        /// </SUMMARY>
        /// <PARAM name="lpEnumFunc">EnumWindows의 실행 결과를 받아줄 콜백함수이다.
        /// EnumWindows는 이 함수 결과가 false가 될 때까지 계속 윈도우를 검색하게 된다.</PARAM>
        /// <PARAM name="lParam"></PARAM>
        /// <RETURNS></RETURNS>
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumDelegate lpEnumFunc, Int32 lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumDelegate lpEnumFunc, IntPtr lParam);

        /// <summary>
        /// enumarator on all desktop windows
        /// </summary>
        /// <param name="hDesktop"></param>
        /// <param name="lpEnumCallbackFunction"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
            ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);




        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parentWindow, IntPtr previousChildWindow, string windowClass, string windowTitle);


        /// <SUMMARY>
        /// HWND 값을 이용하여 프로세스 ID를 알려주는 함수이다.
        /// </SUMMARY>
        /// <PARAM name="hWnd"></PARAM>
        /// <PARAM name="lpdwProcessId"></PARAM>
        /// <RETURNS></RETURNS>
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, ref Int32 lpdwProcessId);

        /// <SUMMARY>
        /// 윈도우의 캡션을 가져온다.
        /// </SUMMARY>
        /// <PARAM name="hWnd">Window의 Handle</PARAM>
        /// <PARAM name="lpString"></PARAM>
        /// <PARAM name="nMaxCount"></PARAM>
        /// <RETURNS></RETURNS>+
        /// 
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, Int32 nMaxCount);

        /// <summary>
        /// Window의 활성화 여부를 확인한다.
        /// </summary>
        /// <param name="hWnd">확인 할 Window의 Handle</param>
        /// <returns>활성 여부</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>
        /// 창의 표시 상태를 설정한다.
        /// </summary>
        /// <param name="hWnd">창의 Handle</param>
        /// <param name="nCmdShow">설정할 상태(ShowWindow의 상태 참조)</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        /// <summary>
        /// 다른 스레드로부터 생성된 창의 표시 상태를 설정한다.
        /// </summary>
        /// <param name="hWnd">창의 Handle</param>
        /// <param name="nCmdShow">설정할 상태(ShowWindow의 상태 참조)</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr LoadCursorFromFile(string str);



        /// <summary>
        /// 창의 제목과 일치하는 창의 Handle을 구한다.
        /// </summary>
        /// <param name="caption">창의 제목</param>
        /// <returns>창의 Handle, 일치하는 창이 없을 경우 IntPtr.Zero를 반환한다.</returns>
        /// <param name="onlyVisible">활성화되어 있는 창만 검색할 지 여부</param>
        public static IntPtr GetWindowHandle(string caption, bool onlyVisible)
        {
            IntPtr retVal = IntPtr.Zero;

            EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
            {
                // 일치하는 창을 찾은 경우 false를 반환 검색을 중지한다.
                StringBuilder strbTitle = new StringBuilder(255);
                int nLength = GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                if (nLength == 0)
                    return true;

                string strTitle = strbTitle.ToString();
                if (onlyVisible)
                {
                    // 현재 활성화되어 있는 창 중에서만 찾을 때
                    if (IsWindowVisible(hWnd) && string.Equals(caption, strTitle))
                    {
                        retVal = hWnd;
                        return false;
                    }
                }
                else
                {
                    // 모든 창 중에서만 찾을 때.
                    if (string.Equals(caption, strTitle))
                    {
                        retVal = hWnd;
                        return false;
                    }
                }
                return true;
            };

            EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero);
            return retVal;
        }



        //BOOL WINAPI SetWindowPos(
        //  __in      HWND hWnd,
        //  __in_opt  HWND hWndInsertAfter,
        //  __in      int X,
        //  __in      int Y,
        //  __in      int cx,
        //  __in      int cy,
        //  __in      UINT uFlags
        //);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetWindowPos(HandleRef hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        public static bool SetWindowPos(HandleRef hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, WM.SizeFlag uFlags)
        {
            return SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, (uint)uFlags);
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("user32.dll")]
        public static extern int GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINTSTRUCT lpPoint);

        [DllImport("user32.dll", EntryPoint = "IsChild")]
        public static extern bool IsChildWindow(IntPtr hWndParent, IntPtr hwnd);

        /// <summary>
        /// 
        /// </summary>
        public struct FlashWindowInfo
        {
            public uint cbSize;
            public IntPtr hWnd;
            public int dwFlags;
            public uint uCount;
            public int dwTimeout;
        }

        /// <summary>
        /// 윈도우 이벤트 정보
        /// </summary>
        public enum ShowWindows : short
        {
            SW_HIDE = 0,
            SW_MAXIMIZE = 3,
            SW_MINIMIZE = 6,
            SW_RESTORE = 9,
            SW_SHOW = 5,
            SW_SHOWDEFAULT = 10,
            SW_SHOWMAXIMIZED = 3,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOWNORMAL = 1,
        }

        /// <summary>
        /// 마지막에 입력된 윈도우 이벤트 정보
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public class LastInputInfo
        {
            [FieldOffset(0)]
            public uint cbSize = 8;
            [FieldOffset(4)]
            public uint dwTime;
        }

        public class Bit
        {
            public static int HiWord(int iValue)
            {
                return ((iValue >> 16) & 0xFFFF);
            }
            public static int LoWord(int iValue)
            {
                return (iValue & 0xFFFF);
            }
        }

        //[DllImport("user32.dll")]
        //private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        //[DllImport("user32.dll")]
        //private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        //private static int keyId;
        //public static void RegisterHotKey(Form form, Keys key)
        //{
        //    int modifiers = 0;

        //    if ((key & Keys.Alt) == Keys.Alt)
        //        modifiers = modifiers | WM.MOD_ALT;

        //    if ((key & Keys.Control) == Keys.Control)
        //        modifiers = modifiers | WM.MOD_CONTROL;

        //    if ((key & Keys.Shift) == Keys.Shift)
        //        modifiers = modifiers | WM.MOD_SHIFT;

        //    Keys k = key & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;

        //    Func ff = delegate()
        //    {
        //        keyId = form.GetHashCode(); // this should be a key unique ID, modify this if you want more than one hotkey
        //        RegisterHotKey((IntPtr)form.Handle, keyId, modifiers, (int)k);
        //    };

        //    form.Invoke(ff); // this should be checked if we really need it (InvokeRequired), but it's faster this way
        //}

        //private delegate void Func();

        //public static void UnregisterHotKey(Form form)
        //{
        //    try
        //    {
        //        Func ff = delegate()
        //        {
        //            UnregisterHotKey(form.Handle, keyId); // modify this if you want more than one hotkey
        //        };

        //        form.Invoke(ff); // this should be checked if we really need it (InvokeRequired), but it's faster this way
        //    }
        //    catch (Exception ex)
        //    {
        //        //Debug.WriteLine(ex.ToString());
        //    }
        //}

        #region DWM API -------------------------------------------------------
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, DWM_BLURBEHIND pBlurBehind);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, MARGINS pMargins);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmGetColorizationColor(
            out int pcrColorization,
            [MarshalAs(UnmanagedType.Bool)] out bool pfOpaqueBlend);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableComposition(bool bEnable);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern IntPtr DwmRegisterThumbnail(IntPtr dest, IntPtr source);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmUnregisterThumbnail(IntPtr hThumbnail);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmUpdateThumbnailProperties(IntPtr hThumbnail, DWM_THUMBNAIL_PROPERTIES props);


        [StructLayout(LayoutKind.Sequential)]
        public class DWM_THUMBNAIL_PROPERTIES
        {
            public uint dwFlags;
            public RECT rcDestination;
            public RECT rcSource;
            public byte opacity;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fVisible;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fSourceClientAreaOnly;

            public const uint DWM_TNP_RECTDESTINATION = 0x00000001;
            public const uint DWM_TNP_RECTSOURCE = 0x00000002;
            public const uint DWM_TNP_OPACITY = 0x00000004;
            public const uint DWM_TNP_VISIBLE = 0x00000008;
            public const uint DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MARGINS
        {
            public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight;

            public MARGINS(int left, int top, int right, int bottom)
            {
                cxLeftWidth = left; cyTopHeight = top;
                cxRightWidth = right; cyBottomHeight = bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DWM_BLURBEHIND
        {
            public uint dwFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fEnable;
            public IntPtr hRegionBlur;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fTransitionOnMaximized;

            public const uint DWM_BB_ENABLE = 0x00000001;
            public const uint DWM_BB_BLURREGION = 0x00000002;
            public const uint DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left; this.Top = top; this.Right = right; this.Bottom = bottom;
            }
        }

        public struct NCCALCSIZE_PARAMS
        {
            public RECT rcNewWindow;
            public RECT rcOldWindow;
            public RECT rcClient;
            IntPtr lppos;
        }
        #endregion DWM API ----------------------------------------------------

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr IParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct TRACKMOUSEEVENT
        {
            public int cbSize;
            public int dwFlags;
            public IntPtr hwndTrack;
            public int dwHoverTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINTSTRUCT
        {
            public int x;
            public int y;

            public POINTSTRUCT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [Serializable]
        public struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int pt_x;
            public int pt_y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;

            public POINT()
            {
            }

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class SCROLLINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(SCROLLINFO));
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }

        #region 파일 확장자에 따른 아이콘 구하기 -----------------------------------
        /// <summary>
        /// Structure that encapsulates basic information of icon embedded in a file.
        /// </summary>
        public struct EmbeddedIconInfo
        {
            public string FileName;
            public int IconIndex;
        }

        /// <summary>
        /// Gets registered file types and their associated icon in the system.
        /// </summary>
        /// <returns>Returns a hash table which contains the file extension as keys, the icon file and param as values.</returns>
        public static Hashtable GetFileTypeAndIcon()
        {
            try
            {
                // Create a registry key object to represent the HKEY_CLASSES_ROOT registry section
                RegistryKey rkRoot = Registry.ClassesRoot;

                //Gets all sub keys' names.
                string[] keyNames = rkRoot.GetSubKeyNames();
                Hashtable iconsInfo = new Hashtable();

                //Find the file icon.
                foreach (string keyName in keyNames)
                {
                    if (String.IsNullOrEmpty(keyName))
                        continue;
                    int indexOfPoint = keyName.IndexOf(".");

                    //If this key is not a file exttension(eg, .zip), skip it.
                    if (indexOfPoint != 0)
                        continue;

                    RegistryKey rkFileType = rkRoot.OpenSubKey(keyName);
                    if (rkFileType == null)
                        continue;

                    //Gets the default value of this key that contains the information of file type.
                    object defaultValue = rkFileType.GetValue("");
                    if (defaultValue == null)
                        continue;

                    //Go to the key that specifies the default icon associates with this file type.
                    string defaultIcon = defaultValue.ToString() + "\\DefaultIcon";
                    RegistryKey rkFileIcon = rkRoot.OpenSubKey(defaultIcon);
                    if (rkFileIcon != null)
                    {
                        //Get the file contains the icon and the index of the icon in that file.
                        object value = rkFileIcon.GetValue("");
                        if (value != null)
                        {
                            //Clear all unecessary " sign in the string to avoid error.
                            string fileParam = value.ToString().Replace("\"", "");
                            iconsInfo.Add(keyName.ToLower(), fileParam);
                        }
                        rkFileIcon.Close();
                    }
                    rkFileType.Close();
                }
                rkRoot.Close();
                return iconsInfo;
            }
            catch (Exception exc)
            {
                //throw exc;
                return null;
            }
        }

        /// <summary>
        /// Parses the parameters string to the structure of EmbeddedIconInfo.
        /// </summary>
        /// <param name="fileAndParam">The params string, 
        /// such as ex: "C:\\Program Files\\NetMeeting\\conf.exe,1".</param>
        /// <returns></returns>
        private static EmbeddedIconInfo getEmbeddedIconInfo(string fileAndParam)
        {
            EmbeddedIconInfo embeddedIcon = new EmbeddedIconInfo();

            if (String.IsNullOrEmpty(fileAndParam))
                return embeddedIcon;

            //Use to store the file contains icon.
            string fileName = String.Empty;

            //The index of the icon in the file.
            int iconIndex = 0;
            string iconIndexString = String.Empty;

            int commaIndex = fileAndParam.IndexOf(",");
            //if fileAndParam is some thing likes that: "C:\\Program Files\\NetMeeting\\conf.exe,1".
            if (commaIndex > 0)
            {
                fileName = fileAndParam.Substring(0, commaIndex);
                iconIndexString = fileAndParam.Substring(commaIndex + 1);
            }
            else
                fileName = fileAndParam;

            if (!String.IsNullOrEmpty(iconIndexString))
            {
                //Get the index of icon.
                iconIndex = int.Parse(iconIndexString);
                if (iconIndex < 0)
                    iconIndex = 0;  //To avoid the invalid index.
            }

            embeddedIcon.FileName = fileName;
            embeddedIcon.IconIndex = iconIndex;

            return embeddedIcon;
        }

        #endregion 파일 확장자에 따른 아이콘 구하기 --------------------------------

        #region Capture API

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("gdi32.dll")]
        public static extern IntPtr DeleteDC(IntPtr hDc);
        [DllImport("gdi32.dll")]
        public static extern IntPtr DeleteObject(IntPtr hDc);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

        #endregion

        #region Windows Task bar 관련 Win API

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);
        //private static string AppID = "3232323232"; // use the same ID in all 5 apps
        //private Security.SecuFileHandler _secuHandler;

        //[DllImport("shell32.dll", SetLastError = true)]
        //public static extern void SetCurrentProcessExplicitAppUserModelID(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string AppID);

        [DllImport("shell32.dll")]
        public static extern void GetCurrentProcessExplicitAppUserModelID([Out(), MarshalAs(UnmanagedType.LPWStr)] out string AppID);
        #endregion

        #region Clipboard

        /*
         * 작성자: jeon
         * 설명: Clipboard에서 HTML 형태의 데이터를 가져올 때, 
         * 일반적인 Clipboard.GetData()를 사용하면 Encoding의 차이로
         * 데이터를 제대로 가져올 수 없다.
         */

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        [DllImport("user32.dll")]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [DllImport("user32.dll")]
        public static extern bool EmptyClipboard();
        [DllImport("user32.dll")]
        public static extern bool CloseClipboard();
        [DllImport("user32.dll")]
        public static extern IntPtr GetClipboardData(IntPtr uFormat);
        [DllImport("user32.dll", EntryPoint = "RegisterClipboardFormatA")]
        public static extern IntPtr RegisterClipboardFormat(string strFormatName);
        [DllImport("kernel32.dll")]
        static extern UIntPtr GlobalSize(IntPtr hMem);
        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalLock(IntPtr hMem);

        /// <summary>
        /// Clipboard에서 HTML 데이터를 구한다.
        /// </summary>
        /// <param name="handle">Window Handle</param>
        /// <returns>HTML String</returns>
        public static string GetHTMLClipboard(IntPtr handle)
        {
            string sHtmlCode = string.Empty;

            if (false != OpenClipboard(handle)) //클립보드 열기.
            {
                IntPtr hMemHandle, nHtmlFormat;
                nHtmlFormat = RegisterClipboardFormat("HTML Format"); //HTML Format 등록 및 해당 포인터 받아오기.

                // Get pointer to clipboard data in the selected format
                hMemHandle = GetClipboardData(nHtmlFormat);

                if (0 != (int)hMemHandle) // 자료 체크...
                {
                    // Do a bunch of crap necessary to copy the data from th memory
                    // th above pointer point at to a place we can access if.
                    UIntPtr length = GlobalSize(hMemHandle);
                    IntPtr gLock = GlobalLock(hMemHandle);

                    // Init a buffer chich will contain the clipbard data
                    byte[] buffer = new byte[(int)length];

                    // Copy clipboard data to buffer
                    Marshal.Copy(gLock, buffer, 0, (int)length);

                    // utf-8 Encoding
                    sHtmlCode = Encoding.UTF8.GetString(buffer);
                }

                CloseClipboard(); // 클리보드 닫기.

                //// Html 소스 저장 구조가 좀 복잡한데.. <body> 내부의 값만을 가져오기 위한 작업입니다....
                //int startPos = sHtmlCode.IndexOf("<!--StartFragment-->") + 20;
                //int endPos = sHtmlCode.IndexOf("<!--EndFragment-->");
                //if (startPos < endPos)
                //    sHtmlCode = sHtmlCode.Substring(startPos, endPos - startPos);// 필요한 Html Source로 만든다. 
            }

            return sHtmlCode;
        }


        //// 고민 끝에 만든건데..  제 기능을 하긴 합니다. 하지만 확실치 않으니 사용시 주의 하세요..
        //public static int GetClipboardDataSize(IntPtr start)
        //{
        //    int pLength = 0;
        //    int offset = 1;
        //    while (0 < Marshal.ReadByte(start))
        //    {
        //        pLength++;
        //        start = (IntPtr)(((int)start) + offset);
        //    }

        //    return pLength;
        //}

        #endregion
    }
}
