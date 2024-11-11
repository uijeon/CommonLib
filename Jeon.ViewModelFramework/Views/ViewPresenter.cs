using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Jeon.ViewModelFramework.Interface;

namespace Jeon.ViewModelFramework.Views
{
	/// <summary>
	/// MainView 에서 View를 표시할 사용자 Content Control
	/// 사용시 *.xaml Style 지정 필요
	/// </summary>
	[ContentProperty("Content")]
	public class ViewPresenter : Control
	{
		#region Dependency Properties
		public static readonly DependencyProperty ContentProperty;
		static void RaiseContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ViewPresenter)d).RaiseContentChanged(e.OldValue, e.NewValue);
		}
		#endregion //Dependency Properties

		/// <summary>
		/// Content Root
		/// </summary>
		ContentPresenter root;
		/// <summary>
		/// 화면에 표시 될 View(Content)
		/// </summary>
		public object Content
		{
			get { return GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}


		static ViewPresenter()
		{
			Type ownerType = typeof(ViewPresenter);
			ContentProperty = DependencyProperty.Register("Content", typeof(object), ownerType, new PropertyMetadata(null, RaiseContentChanged));
		}
		public ViewPresenter()
		{
			this.DefaultStyleKey = typeof(ViewPresenter);
			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
		}


		protected virtual void SubscribeToViewIsReadyToAppearChanged(object view, EventHandler handler)
		{
			IControlView v = view as IControlView;
			if (v != null)
				v.ViewIsReadyToAppearChanged += handler;
		}
		protected virtual void UnsubscribeFromViewIsReadyToAppearChanged(object view, EventHandler handler)
		{
			IControlView v = view as IControlView;
			if (v != null)
				v.ViewIsReadyToAppearChanged -= handler;
		}
		protected virtual bool ViewIsReadyToAppear(object view)
		{
			IControlView v = view as IControlView;
			return v == null ? true : v.ViewIsReadyToAppear;
		}
		protected virtual void SetViewIsVisible(object view, bool value)
		{
			IControlView v = view as IControlView;
			if (v != null)
				v.SetViewIsVisible(value);
		}
		protected virtual void RaiseBeforeViewDisappear(object view)
		{
			IControlView v = view as IControlView;
			if (v != null)
				v.RaiseBeforeViewDisappear();
		}
		protected virtual void RaiseAfterViewDisappear(object view)
		{
			IControlView v = view as IControlView;
			if (v != null)
				v.RaiseAfterViewDisappear();
		}
		protected virtual void OnLoaded(object sender, RoutedEventArgs e)
		{
			BuildVisualTree();
		}
		protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
		{
			ClearVisualTree();
		}
		void BuildVisualTree()
		{
		}
		void ClearVisualTree()
		{
			if (this.root != null)
				this.root.Content = null;
		}
		void RaiseContentChanged(object oldValue, object newValue)
		{
			IControlView OldValue = oldValue as IControlView, NewValue = newValue as IControlView;
			if (OldValue != null)
			{
				RaiseBeforeViewDisappear(OldValue);
				RaiseAfterViewDisappear(OldValue);
			}

			if (this.root != null)
				this.root.Content = NewValue;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.root = (ContentPresenter)GetTemplateChild("Root");
			BuildVisualTree();
		}
	}
}
