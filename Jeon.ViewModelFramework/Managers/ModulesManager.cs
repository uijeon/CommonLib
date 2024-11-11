using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Jeon.ViewModelFramework.Interface;
using Jeon.ViewModelFramework.ViewModels;

namespace Jeon.ViewModelFramework.Managers
{
	/// <summary>
	/// Main에서 등록 할 수 있는 모델 객체
	/// </summary>
	public class ModuleDescription
	{
		public Type ModuleType { get; set; }
		public Type ViewType { get; set; }
		public Type NavigationParentModuleType { get; set; }
	}
	/// <summary>
	/// Main에서 가질 수 있는 모델 객체 컬렉션
	/// </summary>
	public class ModuleDescriptionCollection : List<ModuleDescription>, IViewsListProvider, IModulesListProvider
	{
		Type mainModuleType = null;
		public Type GetMainModuleType()
		{
			if (mainModuleType == null)
				mainModuleType = GetMainModuleTypeCore();
			return mainModuleType;
		}
		Type GetMainModuleTypeCore()
		{
			return (from t in this where t.ModuleType != null && t.ModuleType.IsSubclassOf(typeof(BaseMainControlViewModel)) select t.ModuleType).SingleOrDefault();
		}
		Type IModulesListProvider.GetNavigationParentModuleType(Type moduleType)
		{
			if (moduleType == null || moduleType == GetMainModuleType()) return null;
			Type navigationParentModuleType = (from t in this where t.ModuleType == moduleType select t.NavigationParentModuleType).SingleOrDefault();
			return navigationParentModuleType ?? GetMainModuleType();
		}
		Type IViewsListProvider.GetViewType(Type moduleType)
		{
			if (moduleType == null) return null;
			return (from t in this where t.ModuleType == moduleType select t.ViewType).SingleOrDefault();
		}
	}

	/// <summary>
	/// Thread AutoResetEvent 지원
	/// </summary>
	public class ModulesManagerInternalData { }

	/// <summary>
	/// 모델 객체 생성 및 View 바인딩 매니저
	/// </summary>
	public class ModulesManager : IModulesManager
	{
		class InternalData : ModulesManagerInternalData
		{
			public static InternalData Get(IModule module)
			{
				return (InternalData)module.ModulesManagerInternalData;
			}
			public InternalData()
			{
				Loaded = new AutoResetEvent(false);
			}
			public AutoResetEvent Loaded;
		}
		/// <summary>
		/// 모델 생성 후 Main UI Thread 로 연결하기 위한 객체
		/// </summary>
		class ModuleCreator
		{
			IModule module;
			object parameter;
			IViewsManager viewsManager;
			List<IModule> submodules;
			public ModuleCreator(IModule module, object parameter, IViewsManager viewsManager)
			{
				this.module = module;
				this.parameter = parameter;
				this.viewsManager = viewsManager;
			}
			public void Load()
			{
				this.module.InitParam = this.parameter;
				this.module.RaiseBeforeAppearAsync();
			}
			public void InitModule()
			{
				this.module.RaiseBeforeAppear();
				this.submodules = module.GetSubModules();
				BackgroundHelper.DoInBackground(WaitSubmodules, ShowModule);
			}
			void WaitSubmodules()
			{
				foreach (IModule submodule in this.submodules)
				{
					if (submodule == null) continue;
					InternalData.Get(submodule).Loaded.WaitOne();
				}
				this.submodules = null;
			}
			void ShowModule()
			{
				viewsManager.ShowView(module);
				InternalData.Get(module).Loaded.Set();
			}
		}

		IModulesListProvider modulesListProvider;
		Dictionary<Type, IModule> persistentModules = new Dictionary<Type, IModule>();

		/// <summary>
		/// View Creator Manager
		/// </summary>
		public IViewsManager ViewsManager { get; private set; }

		public ModulesManager(IViewsManager viewsManager, IModulesListProvider modulesListProvider)
		{
			this.modulesListProvider = modulesListProvider;
			ViewsManager = viewsManager;
		}

		/// <summary>
		/// 모델 생성
		/// </summary>
		/// <param name="moduleType"></param>
		/// <param name="module"></param>
		/// <param name="parent"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public IModule CreateModule(Type moduleType, IModule module, IModule parent, object parameter = null)
		{
			MethodInfo createModuleCoreMethod = GetType().GetMethod("CreateModuleCore", BindingFlags.NonPublic | BindingFlags.Instance);
			MethodInfo createModuleMethod = createModuleCoreMethod.MakeGenericMethod(moduleType);
			return (BaseControlViewModel)createModuleMethod.Invoke(this, new object[] { module, parent, parameter });
		}
		/// <summary>
		/// 모델 생성
		/// </summary>
		/// <typeparam name="TModule"></typeparam>
		/// <param name="module"></param>
		/// <param name="parent"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public TModule CreateModule<TModule>(TModule module, IModule parent, object parameter = null) where TModule : class, IModule, new()
		{
			return CreateModuleCore<TModule>(module, parent, parameter);
		}
		public Type GetNavigationParentModuleType(Type moduleType)
		{
			return modulesListProvider.GetNavigationParentModuleType(moduleType);
		}
		public bool IsNavigationParent(Type moduleType, Type navigationParentModuleType)
		{
			for (Type t = moduleType; t != null; t = modulesListProvider.GetNavigationParentModuleType(t))
			{
				if (t == navigationParentModuleType) return true;
			}
			return false;
		}
		/// <summary>
		/// 모델에 맞는 View를 생성하고 화면에 표시함
		/// </summary>
		/// <typeparam name="TModule"></typeparam>
		/// <param name="module"></param>
		/// <param name="parent"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		protected internal TModule CreateModuleCore<TModule>(TModule module, IModule parent, object parameter) where TModule : class, IModule, new()
		{
			if (module == null)
				module = GetPersistentModule<TModule>();
			if (module == null)
			{
				module = new TModule();
				module.BeginInit();
				module.ModulesManagerInternalData = new InternalData();
				module.SetModulesManager(this);
				module.SetParent(parent);
				ViewsManager.CreateView(module);
				module.EndInit();
				if (module.IsPersistentModule)
					SavePersistentModule(module);
			}
			else
			{
				ViewsManager.CreateView(module);
			}
			ModuleCreator creator = new ModuleCreator(module, parameter, ViewsManager);
			BackgroundHelper.DoInBackground(creator.Load, creator.InitModule);
			return module;
		}
		protected TModule GetPersistentModule<TModule>() where TModule : class, IModule, new()
		{
			IModule persistentModule;
			return persistentModules.TryGetValue(typeof(TModule), out persistentModule) ? (TModule)persistentModule : null;
		}
		protected void SavePersistentModule<TModule>(TModule module) where TModule : class, IModule, new()
		{
			if (persistentModules.ContainsKey(typeof(TModule)))
				persistentModules[typeof(TModule)] = module;
			else
				persistentModules.Add(typeof(TModule), module);
		}
		void IModulesManager.RemovePersistentModule(Type moduleType)
		{
			if (persistentModules.ContainsKey(moduleType))
				persistentModules.Remove(moduleType);
		}
	}
}
