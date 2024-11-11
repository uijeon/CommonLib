using Jeon.CommonSampleWindow.ViewModels.Bases;
using Jeon.CommonSampleWindow.WindowManagers.Enums;
using Jeon.CommonSampleWindow.WindowManagers.Interfaces;
using Jeon.ViewModelFramework.RelayCommands;
using System.Windows;
using System.Windows.Input;

namespace Jeon.CommonSampleWindow.ViewModels
{
	public class CommonWindowViewModel : BaseViewModel, IWindowViewModel
	{
		#region Fields

		private RelayCommand<IWindowView> _closeCommand;

		private RelayCommand<IWindowView> _minimizeCommand;

		private RelayCommand<IWindowView> _normalOrMaximizeCommand;

		#endregion

		#region Properties

		public IWindowContent ViewContext { get; set; }

		public ResizeMode WindowResizeMode { get; set; }

		public SizeToContent WindowSizeToContent { get; set; }

		public WindowStyle HeaderStyle { get; set; }

		public WindowState CurrentViewState { get; set; }

		public double BaseWidth { get; set; }

		public double BaseHeight { get; set; }

		public string BaseTitle { get; set; }

		#region Commands

		public ICommand CloseCommand =>
			this._closeCommand ?? (this._closeCommand = new RelayCommand<IWindowView>(this.Close));

		public ICommand MinimizeCommand =>
			this._minimizeCommand ?? (this._minimizeCommand = new RelayCommand<IWindowView>(this.Minimize));

		public ICommand NormalOrMaximizeCommand =>
			this._normalOrMaximizeCommand ??
			(this._normalOrMaximizeCommand = new RelayCommand<IWindowView>(this.NormalOrMaximize));

		#endregion

		#endregion

		#region Methods

		protected void Close(IWindowView window)
		{
			this.ViewContext.Close();

			this.WindowManagerService.CloseWindow(window);
		}

		protected void Minimize(IWindowView window)
		{
			this.WindowManagerService.ChangeWindowState(window, WindowState.Minimized);
		}

		protected void NormalOrMaximize(IWindowView window)
		{
			var toState = WindowState.Normal;
			switch (this.CurrentViewState)
			{
				case WindowState.Minimized:
				case WindowState.Maximized:
					toState = WindowState.Normal;
					break;
				case WindowState.Normal:
					toState = WindowState.Maximized;
					break;
			}

			this.CurrentViewState = toState;

			this.WindowManagerService.ChangeWindowState(window, toState);
		}

		#endregion
	}
}
