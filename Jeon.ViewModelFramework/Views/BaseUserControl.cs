using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using Jeon.ViewModelFramework.Interface;

namespace Jeon.ViewModelFramework.Views
{
	/// <summary>
	/// View Content 생성 및 Dispose 를 포함하는 UserControl
	/// </summary>
	public class BaseUserControl : System.Windows.Controls.UserControl, IControlView, IModuleView, INotifyPropertyChanged, IDisposable
	{
		/// <summary>
		/// The ThreadBarrier's captured SynchronizationContext
		/// </summary>
		private readonly SynchronizationContext _synchronizationContext = AsyncOperationManager.SynchronizationContext;

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler ViewIsReadyToAppearChanged;
		public event EventHandler ViewIsVisibleChanged;
		public event EventHandler BeforeViewDisappear;
		public event EventHandler AfterViewDisappear;

		#region Dependency Properties
		public static readonly DependencyProperty ViewIsReadyToAppearProperty;
		public static readonly DependencyProperty ViewIsVisibleProperty;

		static void RaiseViewIsReadyToAppearChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BaseUserControl)d).RaiseViewIsReadyToAppearChanged(e);
		}
		static void RaiseViewIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BaseUserControl)d).RaiseViewIsVisibleChanged(e);
		}
		#endregion //Dependency

		#region Properties

		public System.Windows.Controls.UserControl UserControl
		{
			get { return this; }
		}

		public bool ViewIsReadyToAppear
		{
			get { return (bool)GetValue(ViewIsReadyToAppearProperty); }
			private set
			{
				if (this._synchronizationContext != null)
					this._synchronizationContext.Send(delegate { SetValue(ViewIsReadyToAppearProperty, value); }, null);
				else
					SetValue(ViewIsReadyToAppearProperty, value);
			}
		}
		public bool ViewIsVisible
		{
			get { return (bool)GetValue(ViewIsVisibleProperty); }
			private set
			{
				if (this._synchronizationContext != null)
					this._synchronizationContext.Send(delegate { SetValue(ViewIsVisibleProperty, value); }, null);
				else
					SetValue(ViewIsVisibleProperty, value);
			}
		}

		#endregion //Properties


		public BaseUserControl()
			: base()
		{
		}

		static BaseUserControl()
		{
			Type ownerType = typeof(BaseUserControl);

			ViewIsReadyToAppearProperty = DependencyProperty.Register("ViewIsReadyToAppear", typeof(bool), ownerType, new PropertyMetadata(false, RaiseViewIsReadyToAppearChanged));
			ViewIsVisibleProperty = DependencyProperty.Register("ViewIsVisible", typeof(bool), ownerType, new PropertyMetadata(false, RaiseViewIsVisibleChanged));
		}

		public virtual void Dispose()
		{
			for (int i = 0; i < this.VisualChildrenCount; i++)
			{
				System.Windows.Controls.Border el = this.GetVisualChild(i) as System.Windows.Controls.Border;
				if (el == null) continue;

				System.Windows.Controls.ContentPresenter cp = el.Child as System.Windows.Controls.ContentPresenter;
				if (cp == null) continue;

				System.Windows.Controls.Grid gd = cp.Content as System.Windows.Controls.Grid;
				if (gd == null) continue;
				foreach (var item in gd.Children)
				{
				}
			}
		}


		#region <Events>
		void RaiseViewIsReadyToAppearChanged(DependencyPropertyChangedEventArgs e)
		{
			if (ViewIsReadyToAppearChanged != null)
				ViewIsReadyToAppearChanged(this, EventArgs.Empty);
		}
		void RaiseViewIsVisibleChanged(DependencyPropertyChangedEventArgs e)
		{
			if (ViewIsVisibleChanged != null)
				ViewIsVisibleChanged(this, EventArgs.Empty);
		}
		public void OnPropertyChanged(String Property)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(Property));
		}
		#endregion //Events


		#region <Interface>
		void IControlView.SetViewIsVisible(bool v)
		{
			ViewIsVisible = v;
		}
		void IModuleView.SetViewIsReadyToAppear(bool v)
		{
			ViewIsReadyToAppear = v;
		}
		void IControlView.RaiseBeforeViewDisappear()
		{
			if (BeforeViewDisappear != null)
				BeforeViewDisappear(this, EventArgs.Empty);
		}
		void IControlView.RaiseAfterViewDisappear()
		{
			if (AfterViewDisappear != null)
				AfterViewDisappear(this, EventArgs.Empty);
		}
		#endregion //Interface
	}
}
