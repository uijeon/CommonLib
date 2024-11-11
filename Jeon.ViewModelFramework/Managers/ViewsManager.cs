using System;
using System.Windows;
using System.Windows.Controls;
using Jeon.ViewModelFramework.Interface;

namespace Jeon.ViewModelFramework.Managers
{
	/// <summary>
	/// View 생성을 도와주는 매니저
	/// </summary>
	public class ViewsManager : IViewsManager
	{
		IViewsListProvider viewListProvider;
		public ViewsManager(IViewsListProvider viewListProvider)
		{
			this.viewListProvider = viewListProvider;
		}
		/// <summary>
		/// 모델에 맞는 View 생성(DataContext Binding Model)
		/// </summary>
		/// <param name="module"></param>
		public void CreateView(IModule module)
		{
			FrameworkElement view = (FrameworkElement)module.View;
			if (view == null)
			{
				Type viewType = viewListProvider.GetViewType(module.GetType());
				view = (FrameworkElement)Activator.CreateInstance(viewType);
			}
			view.Opacity = 0.0;
			IModuleView viewAsIModuleView = view as IModuleView;
			if (viewAsIModuleView != null)
			{
				viewAsIModuleView.SetViewIsReadyToAppear(false);
			}
			module.SetView(view);
			view.DataContext = module;
		}
		/// <summary>
		/// 모델에 맞는 View를 표시
		/// </summary>
		/// <param name="module"></param>
		public void ShowView(IModule module)
		{
			FrameworkElement view = (FrameworkElement)module.View;
			if (view == null)
				return;
			IModuleView viewAsIModuleView = view as IModuleView;
			if (viewAsIModuleView != null)
			{
				viewAsIModuleView.BeforeViewDisappear += OnViewBeforeViewDisappear;
				viewAsIModuleView.AfterViewDisappear += OnViewAfterViewDisappear;
				viewAsIModuleView.ViewIsVisibleChanged += OnViewViewIsVisibleChanged;
			}
			view.Opacity = 1.0;
			if (viewAsIModuleView != null)
			{
				viewAsIModuleView.SetViewIsReadyToAppear(true);
			}
		}
		public IModule GetModule(object view)
		{
			FrameworkElement viewAsFrameworkElement = view as FrameworkElement;
			return viewAsFrameworkElement == null ? null : viewAsFrameworkElement.DataContext as IModule;
		}
		void OnViewViewIsVisibleChanged(object sender, EventArgs e)
		{
			FrameworkElement view = (FrameworkElement)sender;
			IModuleView viewAsIModuleView = view as IModuleView;
			IModule module = view.DataContext as IModule;
			if (module != null && viewAsIModuleView != null)
				module.SetIsVisible(viewAsIModuleView.ViewIsVisible);
		}
		void OnViewBeforeViewDisappear(object sender, EventArgs e)
		{
			FrameworkElement view = (FrameworkElement)sender;
			IModule module = view.DataContext as IModule;
			if (module != null)
			{
				foreach (IModule submodule in module.GetSubModules())
				{
					if (submodule == null) continue;
					submodule.RaiseBeforeDisappear();
				}
				module.RaiseBeforeDisappear();
			}
		}
		void OnViewAfterViewDisappear(object sender, EventArgs e)
		{
			FrameworkElement view = (FrameworkElement)sender;
			IModuleView viewAsIModuleView = view as IModuleView;
			IModule module = view.DataContext as IModule;
			if (module != null && module.IsPersistentModule) return;
			if (viewAsIModuleView != null)
			{
				viewAsIModuleView.ViewIsVisibleChanged -= OnViewViewIsVisibleChanged;
				viewAsIModuleView.BeforeViewDisappear -= OnViewBeforeViewDisappear;
				viewAsIModuleView.AfterViewDisappear -= OnViewAfterViewDisappear;
			}
			view.DataContext = null;
			if (module != null)
			{
				foreach (IModule submodule in module.GetSubModules())
				{
					if (submodule == null) continue;
					IModuleView subviewAsIModuleView = submodule.View as IModuleView;
					if (subviewAsIModuleView != null)
						subviewAsIModuleView.RaiseAfterViewDisappear();
				}
				module.SetView(null);
				module.Dispose();
			}
			ContentControl cc = view.Parent as ContentControl;
			if (cc != null)
				cc.Content = null;
			ContentPresenter cp = view.Parent as ContentPresenter;
			if (cp != null)
				cp.Content = null;
		}
	}
}
