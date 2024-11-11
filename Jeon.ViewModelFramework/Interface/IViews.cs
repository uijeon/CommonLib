using System;

namespace Jeon.ViewModelFramework.Interface
{
	/// <summary>
	/// 컨트롤 객체 interface
	/// </summary>
	public interface IControlView
	{
		bool ViewIsReadyToAppear { get; }
		bool ViewIsVisible { get; }
		event EventHandler ViewIsReadyToAppearChanged;
		event EventHandler ViewIsVisibleChanged;
		event EventHandler BeforeViewDisappear;
		event EventHandler AfterViewDisappear;
		void SetViewIsVisible(bool v);
		void RaiseBeforeViewDisappear();
		void RaiseAfterViewDisappear();
	}

	public interface IModuleView : IControlView
	{
		void SetViewIsReadyToAppear(bool v);
	}

	public interface IViewsManager
	{
		void CreateView(IModule module);
		void ShowView(IModule module);
		IModule GetModule(object view);
	}

	public interface IViewsListProvider
	{
		Type GetViewType(Type moduleType);
	}
}
