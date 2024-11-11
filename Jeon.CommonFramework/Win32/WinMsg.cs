using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonFramework.Win32
{
    /// <summary>
    /// Windows Message 입니다.
    /// </summary>
    public struct WM
    {
        #region Document ------------------------------------------------------
        /*
         * 작성자: jeon
         * 설명:
         * WM(Window Message)의 Message 값을 열거한다.
         * 프로그램에서 사용하는 모든 Windows Message는 이곳에 선언하여 사용한다.
         */
        #endregion Document ---------------------------------------------------

        // --------------------------------------------------------------------
        // Window Message
        // --------------------------------------------------------------------
        public const int WH_KEYBOARD_LL = 13;

        public const int WM_SYSCOMMAND = 0x0112;

        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_RESTORE = 0xF120;
        public const int SC_DRAGMOVE = 0xF012;
        public const int SC_MOVE = 0xF010;

        public const int WM_CREATE = 0x0001;
        public const int WM_SIZE = 0x0005;
        public const int WM_CONTEXTMENU = 0x007B;

        public const int WM_ACTIVATE = 0x0006;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_NCACTIVATE = 0x0086;

        public const int WM_SYSTEMDOWN = 0x0104;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;

        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_NCMOUSEMOVE = 0x00A0;
        public const int WM_NCMOUSELEAVE = 0x02A2;
        public const int WM_MOUSELEAVE = 0x02A3;

        public const int TME_HOVER = 0x00000001;
        public const int TME_LEAVE = 0x00000002;
        public const int TME_NONCLIENT = 0x00000010;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;

        public const int WM_IME_SETCONTEXT = 0x0281;

        public const int WM_CUT = 0x0300;
        public const int WM_COPY = 0x0301;
        public const int WM_COPYDATA = 0x004A;
        public const int WM_PASTE = 0x0302;
        public const int WM_CLEAR = 0x0303;
        public const int WM_UNDO = 0x0304;
        public const int WM_RENDERFORMAT = 0x0305;
        public const int WM_RENDERALLFORMATS = 0x0306;
        public const int WM_DESTROYCLIPBOARD = 0x0307;
        public const int WM_DRAWCLIPBOARD = 0x0308;
        public const int WM_PAINTCLIPBOARD = 0x0309;
        public const int WM_VSCROLLCLIPBOARD = 0x030A;
        public const int WM_SIZECLIPBOARD = 0x030B;
        public const int WM_ASKCBFORMATNAME = 0x030C;
        public const int WM_CHANGECBCHAIN = 0x030D;
        public const int WM_HSCROLLCLIPBOARD = 0x030E;
        public const int WM_QUERYNEWPALETTE = 0x030F;
        public const int WM_PALETTEISCHANGING = 0x0310;
        public const int WM_PALETTECHANGED = 0x0311;
        public const int WM_HOTKEY = 0x0312;

        public const int WM_HSCROLL = 0x114;
        public const int WM_VSCROLL = 0x115;

        public const int WM_USER = 0x0400;

        public const int WM_CAFE_SSO = WM_USER + 100;
        /// <summary>
        /// 바로가기
        /// </summary>
        public const int WM_CAFE_SHORTCUT = WM_USER + 101;

        public const int WM_NCCALCSIZE = 0x0083;


        public const int MOD_ALT = 0x0001;
        public const int MOD_CONTROL = 0x0002;
        public const int MOD_SHIFT = 0x0004;
        public const int MOD_WIN = 0x0008;


        // WM_ACTIVATE
        public const int WA_INACTIVE = 0;
        public const int WA_ACTIVE = 1;
        public const int WA_CLICKACTIVE = 2;



        /// <summary>
        /// WM - 
        /// </summary>
        public const int WM_SETFOCUS = 0x0007;

        // --------------------------------------------------------------------
        // Virtual Key
        // --------------------------------------------------------------------
        /// <summary>[Virtual Key] Shift</summary>
        public const int VK_SHIFT = 0x10;     // Shift
        /// <summary>[Virtual Key] Control</summary>
        public const int VK_CONTROL = 0x11;     // Control
        /// <summary>[Virtual Key] Alt</summary>
        public const int VK_MENU = 0x12;     // Alt
        /// <summary>[Virtual Key] Esc</summary>
        public const int VK_ESCAPE = 0x1B;     // Esc
        /// <summary>[Virtual Key] Page Up </summary>
        public const int VK_PRIOR = 0x21;     // Page up
        /// <summary>[Virtual Key] Page Down</summary>
        public const int VK_NEXT = 0x22;     // Page down
        /// <summary>[Virtual Key] End</summary>
        public const int VK_END = 0x23;     // End
        /// <summary>[Virtual Key] Home </summary>
        public const int VK_HOME = 0x24;     // Home
        /// <summary>[Virtual Key] Left arrow </summary>
        public const int VK_LEFT = 0x25;     // Left arrow
        /// <summary>[Virtual Key] Up arrow </summary>
        public const int VK_UP = 0x26;     // Up arrow
        /// <summary>[Virtual Key] Right arrow</summary>
        public const int VK_RIGHT = 0x27;     // Right arrow
        /// <summary>[Virtual Key] Down arrow</summary>
        public const int VK_DOWN = 0x28;     // Down arrow
        /// <summary>[Virtual Key] Down arrow</summary>
        public const int VK_LWIN = 0x5B;     // Down arrow
        /// <summary>[Virtual Key] Down arrow</summary>
        public const int VK_RWIN = 0x5C;     // Down arrow

        public const int VK_PACKET = 0xE7;     // 유니코드 문자를 키입력인것처럼 전달하는 메세지


        public const int SB_HORZ = 0;
        public const int SB_VERT = 1;

        public const int EM_GETSCROLLPOS = WM_USER + 221;
        public const int EM_SETSCROLLPOS = WM_USER + 222;

        public const int EM_GETMODIFY = 0xB8;
        public const int WM_PAINT = 0x0F;
        public const int IME_COMPOSITION = 0x10F;

        public const int OCM_NOTIFY = 0x204E;
        public const int OCM_COMMAND = 0x2111;


        // --------------------------------------------------------------------
        // HitTest
        // --------------------------------------------------------------------
        public const int HT_ERROR = (-2);
        public const int HT_TRANSPARENT = (-1);
        public const int HT_NOWHERE = 0;
        public const int HT_CLIENT = 1;   //Represents the client area of the window
        public const int HT_CAPTION = 2;
        public const int HT_SYSMENU = 3;
        public const int HT_GROWBOX = 4;
        public const int HT_SIZE = HT_GROWBOX;
        public const int HT_MENU = 5;
        public const int HT_HSCROLL = 6;
        public const int HT_VSCROLL = 7;
        public const int HT_MINBUTTON = 8;
        public const int HT_MAXBUTTON = 9;
        public const int HT_LEFT = 10;  //Left border of a window, allows resize horizontally to the left
        public const int HT_RIGHT = 11;  //Right border of a window, allows resize horizontally to the right
        public const int HT_TOP = 12;  //Upper-horizontal border of a window, allows resize vertically up
        public const int HT_TOPLEFT = 13;  //Upper-left corner of a window border, allows resize diagonally to the left
        public const int HT_TOPRIGHT = 14;  //Upper-right corner of a window border, allows resize diagonally to the right
        public const int HT_BOTTOM = 15;  //Lower-horizontal border of a window, allows resize vertically down
        public const int HT_BOTTOMLEFT = 16;  //Lower-left corner of a window border, allows resize diagonally to the left
        public const int HT_BOTTOMRIGHT = 17;  //Lower-right corner of a window border, allows resize diagonally to the right
        public const int HT_BORDER = 18;
        public const int HT_REDUCE = HT_MINBUTTON;
        public const int HT_ZOOM = HT_MAXBUTTON;
        public const int HT_SIZEFIRST = HT_LEFT;
        public const int HT_SIZELAST = HT_BOTTOMRIGHT;

        public const int HT_OBJECT = 19;
        public const int HT_CLOSE = 20;
        public const int HT_HELP = 21;

        // lParam for WM_IME_SETCONTEXT
        /// <summary>Show the candidate window of index 0 by user interface window</summary>
        public const uint ISC_SHOWUICANDIDATEWINDOW = 0x00000001;
        /// <summary>Show the composition window by user interface window</summary>
        public const uint ISC_SHOWUICOMPOSITIONWINDOW = 0x80000000;
        /// <summary></summary>
        public const uint ISC_SHOWUIGUIDELINE = 0x40000000;
        /// <summary></summary>
        public const uint ISC_SHOWUIALLCANDIDATEWINDOW = 0x0000000F;
        /// <summary></summary>
        public const uint ISC_SHOWUIALL = 0xC000000F;

        /// <summary>
        /// TopMost
        /// </summary><remarks> [byJeong] 2020.12.02 </remarks>
        public const int WS_EX_TOPMOST = 0x00000008;

        /// <summary>
        /// NO ACTIVATE
        /// </summary><remarks> [uijeon] 2020.12.02 </remarks>
        public const int WS_EX_NOACTIVATE = 0x08000000;

        public const int WS_THICKFRAME = 0x00040000;

        public const int WS_CAPTION = 0x00C00000;

        public const int WS_MAXIMIZEBOX = 0x00010000;

        public const int SWP_NOSIZE = 0x0001;

        public const int SWP_NOMOVE = 0x0002;

        public const int SWP_FRAMECHANGED = 0x0020;

        // --------------------------------------------------------------------
        // 
        // --------------------------------------------------------------------
        /// <summary>
        /// Animate Window Flag
        /// </summary>
        [Flags]
        public enum AnimateFlag : int
        {
            /// <summary>
            /// Hides the window. By default, the window is shown. 
            /// </summary>
            AW_HIDE = 0x00010000,
            /// <summary>
            /// Activates the window. Do not use this value with AW_HIDE. 
            /// </summary>
            AW_ACTIVATE = 0x00020000,
            /// <summary>
            /// Uses slide animation. By default, roll animation is used. 
            /// This flag is ignored when used with AW_CENTER.
            /// </summary>
            AW_SLIDE = 0x00040000,
            /// <summary>
            /// Uses a fade effect. This flag can be used only if hwnd is a top-level window. 
            /// </summary>
            AW_BLEND = 0x00080000,

            /// <summary>
            /// Makes the window appear to collapse inward if AW_HIDE is used 
            /// or expand outward if the AW_HIDE is not used. The various direction flags have no effect. 
            /// </summary>
            AW_CENTER = 0x00000010,
            /// <summary>
            /// Animates the window from left to right. This flag can be used
            /// with roll or slide animation. It is ignored when used with AW_CENTER or AW_BLEND.
            /// </summary>
            AW_HOR_POSITIVE = 0x00000001,
            /// <summary>
            /// Animates the window from right to left. This flag can be used 
            /// with roll or slide animation. It is ignored when used with AW_CENTER or AW_BLEND.
            /// </summary>
            AW_HOR_NEGATIVE = 0x00000002,
            /// <summary>
            /// Animates the window from top to bottom. This flag can be used 
            /// with roll or slide animation. It is ignored when used with AW_CENTER or AW_BLEND. 
            /// </summary>
            AW_VER_POSITIVE = 0x00000004,
            /// <summary>
            /// Animates the window from bottom to top. This flag can be used 
            /// with roll or slide animation. It is ignored when used with AW_CENTER or AW_BLEND. 
            /// </summary>
            AW_VER_NEGATIVE = 0x00000008
        }

        /// <summary>
        /// Sets a new extended window style. 
        /// </summary>
        public const int GWL_EXSTYLE = -20;
        /// <summary>
        /// Sets a new application instance handle.
        /// </summary>
        public const int GWL_HINSTANCE = -6;
        /// <summary>
        /// Sets a new identifier of the window.
        /// </summary>
        public const int GWL_ID = -12;
        /// <summary>
        /// Sets a new window style.
        /// </summary>
        public const int GWL_STYLE = -16;
        /// <summary>
        /// Sets the user data associated with the window. 
        /// This data is intended for use by the application that created the window.
        /// Its value is initially zero.
        /// </summary>
        public const int GWL_USERDATA = -21;
        /// <summary>
        /// Sets a new address for the window procedure.
        /// </summary>
        public const int GWL_WNDPROC = -4;



        [Flags]
        public enum WindowStyle
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = -2147483648, //0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_CAPTION = 0x00C00000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
            WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),
            WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU),
            WS_CHILDWINDOW = (WS_CHILD)
        }

        public enum FlashFlag
        {
            FLASHW_ALL = 0x00000003,
            FLASHW_CAPTION = 0x00000001,
            FLASHW_STOP = 0x00000000,
            FLASHW_TIMER = 0x00000004,
            FLASHW_TIMERNOFG = 0x0000000C,
            FLASHW_TRAY = 0x00000002
        }


        public const int HWND_BOTTOM = 1;
        public const int HWND_NOTOPMOST = -2;
        public const int HWND_TOP = 0;
        public const int HWND_TOPMOST = -1;

        [Flags]
        public enum SizeFlag : uint
        {
            /// <summary>
            /// If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
            /// </summary>
            SWP_ASYNCWINDOWPOS = 0x4000,
            /// <summary>
            /// Prevents generation of the WM_SYNCPAINT message
            /// </summary>
            SWP_DEFERERASE = 0x2000,
            /// <summary>
            /// Draws a frame (defined in the window's class description) around the window
            /// </summary>
            SWP_DRAWFRAME = 0x0020,
            /// <summary>
            /// Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
            /// </summary>
            SWP_FRAMECHANGED = 0x0020,
            /// <summary>
            /// Hides the window.
            /// </summary>
            SWP_HIDEWINDOW = 0x0080,
            /// <summary>
            /// Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOACTIVATE = 0x0010,
            /// <summary>
            /// Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
            /// </summary>
            SWP_NOCOPYBITS = 0x0100,
            /// <summary>
            /// Retains the current position (ignores X and Y parameters).
            /// </summary>
            SWP_NOMOVE = 0x0002,
            /// <summary>
            /// Does not change the owner window's position in the Z order.
            /// </summary>
            SWP_NOOWNERZORDER = 0x0200,
            /// <summary>
            /// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
            /// </summary>
            SWP_NOREDRAW = 0x0008,
            /// <summary>
            /// Same as the SWP_NOOWNERZORDER flag.
            /// </summary>
            SWP_NOREPOSITION = 0x0200,
            /// <summary>
            /// Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
            /// </summary>
            SWP_NOSENDCHANGING = 0x0400,
            /// <summary>
            /// Retains the current size (ignores the cx and cy parameters).
            /// </summary>
            SWP_NOSIZE = 0x0001,
            /// <summary>
            /// Retains the current Z order (ignores the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOZORDER = 0x0004,
            /// <summary>
            /// Displays the window.
            /// </summary>
            SWP_SHOWWINDOW = 0x0040
        }

        [System.Flags]
        public enum ScrollInfoMask
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS = 0x10,
            SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS
        }
    }
}
