using Jeon.CommonSampleWindow.WindowManagers.Enums;
using Jeon.CommonSampleWindow.WindowManagers.Interfaces;
using System.Windows;

namespace Jeon.CommonSampleWindow.WindowManagers
{
	public class WindowSetting : IWindowSetting
	{
		public ResizeMode WindowResizeMode { get; set; }

		public SizeToContent WindowSizeToContent { get; set; }

		public WindowStyle HeaderStyle { get; set; }

		public WindowState CurrentViewState { get; set; }

		public string BaseTitle { get; set; }

		public double BaseWidth { get; set; }

		public double BaseHeight { get; set; }
	}
}
