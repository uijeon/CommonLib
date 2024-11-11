using System;
using System.Collections.Generic;
using System.ComponentModel;
using Jeon.ViewModelFramework.Managers;

namespace Jeon.ViewModelFramework.Interface
{
	/// <summary>
	/// 모델 객체 interface
	/// </summary>
	public interface IModule : IDisposable, ISupportInitialize
	{
		object View { get; }
		void SetView(object v);
		bool IsPersistentModule { get; }
		List<IModule> GetSubModules();
		//bool IsVisible { get; }
		void SetIsVisible(bool v);
		event EventHandler BeforeDisappear;
		void RaiseBeforeDisappear();
		event EventHandler BeforeAppearAsync;
		void RaiseBeforeAppearAsync();
		event EventHandler BeforeAppear;
		void RaiseBeforeAppear();
		object InitParam { get; set; }
		ModulesManagerInternalData ModulesManagerInternalData { get; set; }
		IModulesManager ModulesManager { get; }
		void SetModulesManager(IModulesManager v);
		IModule Parent { get; }
		IModule Main { get; }
		void SetParent(IModule v);
		void RaiseUpdateDataContext(object parameter);
	}

	public interface IModulesManager
	{
		TModule CreateModule<TModule>(TModule module, IModule parent, object parameter = null) where TModule : class, IModule, new();
		IModule CreateModule(Type moduleType, IModule module, IModule parent, object parameter = null);
		IViewsManager ViewsManager { get; }
		Type GetNavigationParentModuleType(Type moduleType);
		bool IsNavigationParent(Type moduleType, Type navigationParentModuleType);
		void RemovePersistentModule(Type moduleType);
	}

	public interface IModulesListProvider
	{
		Type GetNavigationParentModuleType(Type moduleType);
	}
}
