using Jeon.CommonFramework.ExtensionMethods;
using Jeon.CommonFramework.LocalizationResource;
using Jeon.CommonSampleWindow.Models;
using Jeon.CommonSampleWindow.PubSubEvents;
using Jeon.CommonSampleWindow.PubSubEvents.PusSubEventArgs;
using Jeon.CommonSampleWindow.ViewModels.Bases;
using Jeon.CommonSampleWindow.WindowManagers.Enums;
using Jeon.CommonSampleWindow.WindowManagers.Interfaces;
using Jeon.ViewModelFramework.RelayCommands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jeon.CommonSampleWindow.ViewModels
{
	public class MainViewModel : BaseViewModel, IWindowContent
	{
		private RelayCommand<CultureInfo> _selectCultureCommand;

		private RelayCommand<string> _eventPopupCommand;

		/// <summary>
		/// 선택된 다국어 Item
		/// </summary>
		public CultureInfo SelectedCultureItem { get; set; } = Thread.CurrentThread.CurrentUICulture;

		/// <summary>
		/// 다국어 List
		/// </summary>
		public List<CultureInfo> CultureList { get; set; } = ResourceProviderManager.Instance.SupportedCultures;

		public ICommand SelectCultureCommand => this._selectCultureCommand ?? (this._selectCultureCommand = new RelayCommand<CultureInfo>(p => this.SelectCulture(p)));

		public ICommand EventPopupCommand => this._eventPopupCommand ?? (this._eventPopupCommand = new RelayCommand<string>(p => this.EventPopup(p)));

		public MainViewModel()
		{
			this.CloneTest();
		}

		/// <summary>
		/// DeepCopy 테스트용도
		/// </summary>
		private void CloneTest()
		{
			var setting = new WindowManagers.WindowSetting();

			setting.BaseTitle = "Title";
			setting.BaseHeight = 50;
			setting.BaseWidth = 50;
			setting.CurrentViewState = System.Windows.WindowState.Normal;
			setting.HeaderStyle = System.Windows.WindowStyle.None;
			setting.WindowResizeMode = System.Windows.ResizeMode.CanMinimize;
			setting.WindowSizeToContent = System.Windows.SizeToContent.Height;


			var setting2 = setting.DeepCopyMemberwiseClone<WindowManagers.WindowSetting>();
			setting2.BaseTitle = "Title222";


			var testModel = new TestModel();

			testModel.Title = "First";
			testModel.Second.Number = 10;
			testModel.NumberList = Enumerable.Range(1, 10).ToList();

			var testModel2 = testModel.DeepCopyMemberwiseClone<TestModel>();

			testModel.Title = "Second";

			var dic = new Dictionary<int, string>();

			dic.Add(1, "test");
			dic.Add(2, "test2");
			dic.Add(3, "test3");

			var dic2 = dic.DeepCopyMemberwiseClone<Dictionary<int, string>>();

			foreach(var d in dic2)
			{
				Console.WriteLine($"Key => {d.Key}, Value => {d.Value}");
			}
		}

		/// <summary>
		/// 이벤트 팝업, p 가 널이거나 빈값이면 Arg 없는 이벤트를 퍼블리시 한다.
		/// </summary>
		/// <param name="p"></param>
		private void EventPopup(string p)
		{
			var popup = this.WindowManagerService.GetOrCreateWindow(new EventPopupViewModel(), WindowSettings.Popup);

			if (string.IsNullOrEmpty(p))
			{
				this.EventAggregator.GetEvent<NotArgumentEvent>().Publish();
			}
			else
			{
				this.EventAggregator.GetEvent<ArgumentEvent>().Publish(new MyEventArgs(p));
			}

			popup.Show();
		}

		/// <summary>
		/// 지역화 리소스 변경
		/// </summary>
		private void SelectCulture(CultureInfo culture)
		{
			ResourceProviderManager.Instance.ChangeCulture(culture);
		}

		public void Close()
		{

		}
	}
}
