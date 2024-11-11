using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Jeon.ViewFramework.Utils
{
	public static class ScreenUtil
	{
		private static Point _dpiPoint;

		private static IntPtr _previousLParam;

		static ScreenUtil()
		{
			// 현재 모니터의 Dpi 값을 찾아 보관한다.
			var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
			var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

			var dpiX = (int)dpiXProperty.GetValue(null, null) / 96.0;
			var dpiY = (int)dpiYProperty.GetValue(null, null) / 96.0;

			_dpiPoint = new Point(dpiX, dpiY);
		}

		public static Point GetCurrentMonitorDpi()
		{
			return _dpiPoint;
		}

		public static Rect GetPrimaryScreenRegion()
		{
			return new Rect(Screen.PrimaryScreen.Bounds.Left,
				Screen.PrimaryScreen.Bounds.Top,
				Screen.PrimaryScreen.Bounds.Width,
				Screen.PrimaryScreen.Bounds.Height);
		}

		/// <summary>
		/// 전체 화면 영역을 구하는 함수
		/// </summary>
		/// <returns>전체 화면 영역을 나타내는 값이 반환된다.</returns>
		public static Rect GetTotalScreenBound()
		{
			var resultRect = new Rect();
			var dpiX = _dpiPoint.X;
			var dpiY = _dpiPoint.Y;

			foreach (var screen in Screen.AllScreens)
			{
				resultRect.Union(
					new Rect(
						screen.Bounds.X / dpiX,
						screen.Bounds.Y / dpiY,
						screen.Bounds.Width / dpiX,
						screen.Bounds.Height / dpiY));
			}

			return resultRect;
		}

		/// <summary>
		/// Window Maximize 시 잘림 현상, TaskBar 뒤로 숨는 현상 처리용 Hook 메서드.
		/// </summary>
		/// <param name="window"></param>
		public static void AddHookWindowProcess(Window window)
		{
			var handleSource = GetWindowHwnd(window);
			handleSource?.AddHook(WindowProc);
		}

		public static void RemoveHookWindowProcess(Window window)
		{
			var handleSource = GetWindowHwnd(window);
			handleSource?.RemoveHook(WindowProc);
		}

		private static HwndSource GetWindowHwnd(Window window)
		{
			var handle = new WindowInteropHelper(window).Handle;
			return HwndSource.FromHwnd(handle);
		}

		private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch (msg)
			{
				case 0x0024:/* WM_GETMINMAXINFO */
					if (_previousLParam == lParam) // 너무 잦은 호출 방지.
					{
						break;
					}

					_previousLParam = lParam;

					WmGetMinMaxInfo(hwnd, lParam);
					handled = true;
					break;
			}

			return (IntPtr)0;
		}

		private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
		{
			var mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

			// Adjust the maximized size and position to fit the work area of the correct monitor
			var currentScreen = Screen.FromHandle(hwnd);
			var workArea = currentScreen.WorkingArea;
			var monitorArea = currentScreen.Bounds;

			mmi.ptMaxPosition.x = Math.Abs(workArea.Left - monitorArea.Left);
			mmi.ptMaxPosition.y = Math.Abs(workArea.Top - monitorArea.Top);
			mmi.ptMaxSize.x = Math.Abs(workArea.Right - workArea.Left);
			mmi.ptMaxSize.y = Math.Abs(workArea.Bottom - workArea.Top);

			Marshal.StructureToPtr(mmi, lParam, true);
		}
	}


	[StructLayout(LayoutKind.Sequential)]
	public struct MINMAXINFO
	{
		public POINT ptReserved;
		public POINT ptMaxSize;
		public POINT ptMaxPosition;
		public POINT ptMinTrackSize;
		public POINT ptMaxTrackSize;
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		/// <summary>
		/// x coordinate of point.
		/// </summary>
		public int x;

		/// <summary>
		/// y coordinate of point.
		/// </summary>
		public int y;

		/// <summary>
		/// Construct a point of coordinates (x,y).
		/// </summary>
		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return $"X: {this.x}, Y: {this.y}";
		}
	}
}
