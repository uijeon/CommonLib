using Jeon.CommonSampleWindow.WindowManagers.Enums;
using System.Windows;

namespace Jeon.CommonSampleWindow.WindowManagers.Interfaces
{
	public interface IWindowSetting
	{
		ResizeMode WindowResizeMode { get; set; }

		SizeToContent WindowSizeToContent { get; set; }

		WindowStyle HeaderStyle { get; set; }

		WindowState CurrentViewState { get; set; }

		double BaseWidth { get; set; }

		double BaseHeight { get; set; }

		string BaseTitle { get; set; }
	}
}
