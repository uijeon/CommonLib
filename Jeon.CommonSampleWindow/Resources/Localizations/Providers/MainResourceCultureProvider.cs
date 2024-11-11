using Jeon.CommonFramework.Interfaces;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace Jeon.CommonSampleWindow.Resources.Localizations.Providers
{
    public class MainResourceCultureProvider : ICultureResource
	{
		public MainResourceCultureProvider()
		{
			Resource_Main.Culture = Thread.CurrentThread.CurrentUICulture;
		}

		/// <summary>
		/// The Resources ObjectDataProvider uses this method to get an instance of the DTGApp.Resource.Properties.Resources class
		/// </summary>
		/// <returns></returns>
		public Resource_Main GetResourceInstance()
		{
			return new Resource_Main();
		}

		private ObjectDataProvider _resourceProvider;
		public ObjectDataProvider ResourceProvider
		{
			get
			{
				if (this._resourceProvider == null)
				{
					this._resourceProvider = (ObjectDataProvider)Application.Current.FindResource("Resource_Main");
				}

				return this._resourceProvider;
			}
		}

		/// <summary>
		/// Change the current culture used in the application.
		/// If the desired culture is available all localized elements are updated.
		/// </summary>
		/// <param name="culture">Culture to change to</param>
		public void ChangeCulture(CultureInfo culture)
		{
			if (Resource_Main.Culture == null && Resource_Main.Culture.Equals(culture))
			{
				return;
			}

			Resource_Main.Culture = culture;
			this.ResourceProvider.Refresh();
		}
	}
}
