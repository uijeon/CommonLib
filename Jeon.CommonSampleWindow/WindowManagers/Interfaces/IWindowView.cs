using Jeon.CommonSampleWindow.WindowManagers.Enums;
using System.Windows;

namespace Jeon.CommonSampleWindow.WindowManagers.Interfaces
{
	public interface IWindowView
	{
		void SetOwner(IWindowView windowView);

		void Show();

		bool? ShowDialog();

		bool Activate();

		void MoveWindow();

		void WindowStateChange(WindowState states);

		void Close();

		bool IsClosed { get; }

		void Hide();

		bool? DialogResult { get; set; }
	}
}
