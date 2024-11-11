using Jeon.CommonFramework.Comparer;
using Jeon.CommonSampleWindow.WindowManagers.Enums;
using Jeon.CommonSampleWindow.WindowManagers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Jeon.CommonSampleWindow.WindowManagers
{
	public class WindowManagerService : IWindowManagerService
	{
		#region Fields

		private Type _commonWindowType;

		private readonly List<Type> _customWindowTypes = new List<Type>();

		private readonly Dictionary<WindowSettings, IWindowSetting> _defaultWindowSettings = new Dictionary<WindowSettings, IWindowSetting>(EnumComparer.For<WindowSettings>());

		private readonly Dictionary<IWindowContent, IWindowView> _windowCaches = new Dictionary<IWindowContent, IWindowView>();

		#endregion

		#region Constructors

		public WindowManagerService()
		{
			this.InitializeTypeCache();
			this.InitializeWindowSettings();
		}

		#endregion

		#region Methods

		public IWindowView GetOrCreateWindow(IWindowContent windowContent, WindowSettings setting, IWindowContent ownerViewModel = null, bool isCreate = false)
		{
			if (this._defaultWindowSettings.ContainsKey(setting) == false)
			{
				// NOTE : 예외를 발생시켜야 하나?
				return null;
			}

			var defaultSetting = this._defaultWindowSettings[setting];
			var ownerWindow = this.GetWindowView(ownerViewModel);

			return this.GetOrCreateWindowView(windowContent, defaultSetting, ownerWindow, isCreate);
		}

		public IWindowView GetOrCreateWindow(IWindowContent windowContent, IWindowSetting setting, IWindowContent ownerViewModel = null, bool isCreate = false)
		{
			var ownerWindow = this.GetWindowView(ownerViewModel);
			return this.GetOrCreateWindowView(windowContent, setting, ownerWindow, isCreate); ;
		}

		public IWindowView GetOrCreateWindow<T>(IWindowContent windowContent, IWindowSetting setting, IWindowContent ownerViewModel = null, bool isCreate = false)
		{
			var ownerWindow = this.GetWindowView(ownerViewModel);
			return this.GetOrCreateWindowView(windowContent, setting, ownerWindow, isCreate, typeof(T));
		}

		public void SetOwner(IWindowContent targetViewModel, IWindowContent ownerViewModel)
		{
			var targetWindowView = this.GetWindowView(targetViewModel);
			if (targetWindowView == null)
			{
				return;
			}

			var ownerWindowView = this.GetWindowView(ownerViewModel);
			if (ownerWindowView == null)
			{
				return;
			}

			targetWindowView.SetOwner(ownerWindowView);
		}

		public void SetOwner(IWindowView targetView, IWindowView ownerView)
		{
			targetView.SetOwner(ownerView);
		}

		public void ChangeWindowState(IWindowContent windowViewModel, WindowState states)
		{
			var windowView = this.GetWindowView(windowViewModel);

			windowView?.WindowStateChange(states);
		}

		public void ChangeWindowState(IWindowView windowView, WindowState states)
		{
			windowView.WindowStateChange(states);
		}

		public void DragMove(IWindowContent windowViewModel)
		{
			var windowView = this.GetWindowView(windowViewModel);

			windowView?.MoveWindow();
		}

		public void DragMove(IWindowView windowView)
		{
			windowView.MoveWindow();
		}

		public void CloseWindow(IWindowContent windowViewModel)
		{
			var windowView = this.GetWindowView(windowViewModel);
			if (windowView == null)
			{
				return;
			}

			windowView.Close();

			this._windowCaches.Remove(windowViewModel);

			this.CheckAndRemoveInvalidCache();
		}

		public void HideWindow(IWindowContent windowViewModel)
		{
			var windowView = this.GetWindowView(windowViewModel);
			if (windowView == null)
			{
				return;
			}

			windowView.Hide();
		}

		public void SetDialogResult(IWindowContent windowViewModel, bool? value)
		{
			var windowView = this.GetWindowView(windowViewModel);
			if (windowView == null)
			{
				return;
			}

			windowView.DialogResult = value;
		}

		public void CloseWindow(IWindowView windowView)
		{
			windowView.Close();

			var targetPair = this._windowCaches.FirstOrDefault(w => w.Value.Equals(windowView));
			if (Equals(targetPair, default(KeyValuePair<IWindowContent, IWindowView>)))
			{
				return;
			}

			this._windowCaches.Remove(targetPair.Key);

			this.CheckAndRemoveInvalidCache();
		}

		public void Release()
		{
			foreach (var windowViewPair in this._windowCaches)
			{
				var windowView = windowViewPair.Value;
				windowView.Close();
			}

			this._windowCaches.Clear();
			 
			this._customWindowTypes.Clear();
		}

		private void InitializeTypeCache()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in assemblies)
			{
				// Jeon.CommonSampleWindow 에 포함된 Window만 캐쉬.
				if (string.CompareOrdinal(assembly.ManifestModule.Name, "Jeon.CommonSampleWindow.exe") != 0)
				{
					continue;
				}

				var assemblyTypes = assembly.GetTypes();
				foreach (var type in assemblyTypes)
				{
					if (type.IsInterface)
					{
						continue;
					}

					if (type.Name.EndsWith("Window") == false) // Window name은 항상 Window로 끝난다.
					{
						continue;
					}

					if (type.Name.StartsWith("Base")) // 추상화된 base 개체는 제외한다.
					{
						continue;
					}

					if (type.Name.Contains("CommonWindow"))
					{
						this._commonWindowType = type;
						continue;
					}

					this._customWindowTypes.Add(type);
				}
			}
		}

		private void InitializeWindowSettings()
		{			
			//Main
			var mainSetting = new WindowSetting
			{
				BaseTitle = "Jeon.CommonSample",
				BaseHeight = 480,
				BaseWidth = 640,
				WindowResizeMode = ResizeMode.CanResize,
				HeaderStyle = WindowStyle.ThreeDBorderWindow,
			};

			this._defaultWindowSettings.Add(WindowSettings.Main, mainSetting);

			//Popup
			var popupSetting = new WindowSetting
			{
				BaseTitle = "Event Popup",
				BaseHeight = 240,
				BaseWidth = 320,
				WindowResizeMode = ResizeMode.NoResize,
				HeaderStyle = WindowStyle.SingleBorderWindow,
			};

			this._defaultWindowSettings.Add(WindowSettings.Popup, popupSetting);
		}

		private IWindowView GetOrCreateWindowView(
			IWindowContent windowContent,
			IWindowSetting setting,
			IWindowView ownerWindow,
			bool isCreate,
			Type windowType = null)
		{
			this.CheckAndRemoveInvalidCache();

			var needCreateView = this._windowCaches.ContainsKey(windowContent) == false || isCreate;

			if (needCreateView)
			{
				var windowView = this.CreateWindowView(windowContent, setting, windowType);
				if (windowView == null)
				{
					return null;
				}

				this._windowCaches.Add(windowContent, windowView);
			}

			var window = this._windowCaches[windowContent];

			if (ownerWindow != null)
			{
				window.SetOwner(ownerWindow);
			}

			return window;
		}

		private IWindowView GetWindowView(IWindowContent windowContent)
		{
			if (windowContent == null)
			{
				return null;
			}

			return this._windowCaches.ContainsKey(windowContent) == false ? null : this._windowCaches[windowContent];
		}

		private IWindowView CreateWindowView(IWindowContent windowContent, IWindowSetting setting, Type targeType = null)
		{
			var modelType = windowContent.GetType();
			var windowTypeName = modelType.Name.Replace("ViewModel", string.Empty);

			// 1. 특정 Window 타입이 없으면 CommonWindow 사용. 
			// 2. 1번이 null 일 경우 캐쉬되어 있는 this._customWindowTypes 에서 windowContent 의 이름을 기준으로 찾아서 사용.
			// 3. 2번이 null 일 경우 전달 받은 타입 사용.
			var windowType = (targeType == null ?
				this._commonWindowType :
				this._customWindowTypes.FirstOrDefault(w => string.CompareOrdinal(w.Name, windowTypeName) == 0)) ?? targeType;

			if (windowType == null)
			{
				// TODO : need logging
				return null;
			}

			var windowView = Activator.CreateInstance(windowType) as IWindowView;
			if (windowView == null)
			{
				// TODO : need logging
				return null;
			}

			var dataContextInfo = windowType.GetProperty("DataContext");

			// Window의 DataContext는 View에서 함께 생성한다.
			var dataContext = dataContextInfo?.GetValue(windowView);

			// Window의 ViewModel은 반드시 IWindowViewModel을 상속해야한다.
			var windowViewModel = dataContext as IWindowViewModel;
			if (windowViewModel == null)
			{
				// TODO : need logging
				return null;
			}

			this.AssignWindowSetting(setting, windowViewModel);

			windowViewModel.ViewContext = windowContent;

			return windowView;
		}

		private void AssignWindowSetting(IWindowSetting setting, IWindowSetting windowViewModel)
		{
			windowViewModel.BaseHeight = setting.BaseHeight;
			windowViewModel.BaseTitle = setting.BaseTitle;
			windowViewModel.BaseWidth = setting.BaseWidth;
			windowViewModel.CurrentViewState = setting.CurrentViewState;
			windowViewModel.HeaderStyle = setting.HeaderStyle;
			windowViewModel.WindowResizeMode = setting.WindowResizeMode;
			windowViewModel.WindowSizeToContent = setting.WindowSizeToContent;
		}

		private void CheckAndRemoveInvalidCache()
		{
			var removeList = this._windowCaches.Where(w => w.Value.IsClosed).Select(w => w.Key).ToList();
			foreach (var removeView in removeList)
			{
				this._windowCaches.Remove(removeView);
			}
		}

		#endregion
	}
}
