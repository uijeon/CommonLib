using Jeon.CommonFramework.EventAggregatorParts;
using Jeon.CommonFramework.LocalizationResource;
using N3N.Service.ServiceLocator;
using Jeon.CommonSampleWindow.Resources.Localizations.Providers;
using Jeon.CommonSampleWindow.WindowManagers;
using Jeon.CommonSampleWindow.WindowManagers.Interfaces;

namespace Jeon.CommonSampleWindow.Bootstrap
{
	public class Bootstrapper
	{
		public static void Initialize()
		{
			// TODO : 추후 필요한 것이 있으면 추가.
			var container = ContainerResolver.GetContainer();

			container.RegisterType<IEventAggregator, EventAggregator>();
			container.RegisterType<IWindowManagerService, WindowManagerService>();

			ResourceProviderManager.Instance.Add(new MainResourceCultureProvider());
		}
	}
}
