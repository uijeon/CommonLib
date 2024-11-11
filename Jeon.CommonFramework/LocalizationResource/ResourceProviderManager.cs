using Jeon.CommonFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Jeon.CommonFramework.LocalizationResource
{
	public class ResourceProviderManager
	{
		private static Lazy<ResourceProviderManager> _instance = new Lazy<ResourceProviderManager>(() => new ResourceProviderManager());

		public static ResourceProviderManager Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		public List<ICultureResource> ResourceList { get; } = new List<ICultureResource>();

		public List<CultureInfo> SupportedCultures { get; } = new List<CultureInfo>();

		private ResourceProviderManager()
		{
			this.Init();
		}

		private void Init()
		{
			//determine which cultures are available to this application
			System.Diagnostics.Debug.WriteLine("Get Installed cultures:");

			var entryPath = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) + ".resources.dll";
			var executingPath = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location) +".resources.dll";

			foreach (var dir in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory))
			{
				try
				{
					//see if this directory corresponds to a valid culture name
					var dirinfo = new DirectoryInfo(dir);
					var culture = CultureInfo.GetCultureInfo(dirinfo.Name);

					//determine if a resources dll exists in this directory that matches the executable name
					//리소스용 프로젝트는 따로 생성해야함. 하지만 테스트용도나 간단한 프로그램 제작 시 같은 프로젝트 내의 있을 수 있으므로
					//Entry 쪽도 추가함. 단, Culture 가 중복으로 추가되지는 않음.
					if (dirinfo.GetFiles(entryPath).Length > 0)
					{
						this.SupportedCultures.Add(culture);
						System.Diagnostics.Debug.WriteLine(string.Format(" Found Culture: {0} [{1}]", culture.DisplayName, culture.Name));
						continue;
					}
					else if (dirinfo.GetFiles(executingPath).Length > 0)
					{
						this.SupportedCultures.Add(culture);
						System.Diagnostics.Debug.WriteLine(string.Format(" Found Culture: {0} [{1}]", culture.DisplayName, culture.Name));
					}
				}
				catch (ArgumentException) //ignore exceptions generated for any unrelated directories in the bin folder
				{
				}
			}

			this.ChangeCulture(Thread.CurrentThread.CurrentUICulture);
		}

		public void Add(ICultureResource resource)
		{
			this.ResourceList.Add(resource);
		}

		/// <summary>
		/// Change the current culture used in the application.
		/// If the desired culture is available all localized elements are updated.
		/// </summary>
		/// <param name="culture">Culture to change to</param>
		public void ChangeCulture(CultureInfo culture)
		{
			//remain on the current culture if the desired culture cannot be found
			// - otherwise it would revert to the default resources set, which may or may not be desired.
			if (this.SupportedCultures.Contains(culture))
			{
				foreach (var item in this.ResourceList)
				{
					item.ChangeCulture(culture);
				}

				Thread.CurrentThread.CurrentUICulture = culture;
				Thread.CurrentThread.CurrentCulture = culture;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine(string.Format("Culture [{0}] not available", culture));
			}
		}
	}
}
