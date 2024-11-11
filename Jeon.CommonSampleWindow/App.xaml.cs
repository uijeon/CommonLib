using N3N.Service.ServiceLocator;
using Jeon.CommonSampleWindow.Bootstrap;
using Jeon.CommonSampleWindow.ViewModels;
using Jeon.CommonSampleWindow.WindowManagers.Enums;
using Jeon.CommonSampleWindow.WindowManagers.Interfaces;
using System.Windows;

namespace Jeon.CommonSampleWindow
{
	/// <summary>
	/// App.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			this.ShutdownMode = ShutdownMode.OnMainWindowClose;

			Bootstrapper.Initialize();

			var container = ContainerResolver.GetContainer();
			var windowManager = container.Resolve<IWindowManagerService>();

			var entryWindow = windowManager.GetOrCreateWindow(new MainViewModel(), WindowSettings.Main);

			this.MainWindow = entryWindow as Window;

			entryWindow.Show();
		}
	}
}
