using System;
using System.Collections.Generic;
using Jeon.ViewModelFramework.BaseNotify;
using Jeon.ViewModelFramework.Interface;
using Jeon.ViewModelFramework.Managers;
using Jeon.ViewModelFramework.Views;

namespace Jeon.ViewModelFramework.ViewModels
{
	/// <summary>
	/// Parent, View 를 포함하는 ViewModel
	/// </summary>
	public class BaseControlViewModel : BindableAndDisposable, IModule
	{
		#region <Define>

		#region <Private>
		object _View;
		IModule _Parent;
		IModule _Main;
		bool _IsVisible;
		object initParam;
		ModulesManagerInternalData internalData;
		#endregion //Private

		public bool IsPersistentModule { get; protected set; }
		public IModulesManager ModulesManager { get; protected set; }

		/// <summary>
		/// 데이터 저장 후 발생 이벤트
		/// </summary>
		public event EventHandler BeforeDisappear;
		/// <summary>
		/// 데이터 동기화 후 이벤트
		/// </summary>
		public event EventHandler BeforeAppearAsync;
		/// <summary>
		/// 데이터 초기화 후 이벤트
		/// </summary>
		public event EventHandler BeforeAppear;
		/// <summary>
		/// DataContext Update 이벤트
		/// </summary>
		public event EventHandler<SourceEventArgs> UpdateDataContext;

		/// <summary>
		/// View Content
		/// </summary>
		public object View
		{
			get { return _View; }
			private set { SetValue<object>("View", ref _View, value); }
		}
		public IModule Parent
		{
			get { return _Parent; }
			private set { SetValue<IModule>("Parent", ref _Parent, value); }
		}
		public IModule Main
		{
			get { return _Main; }
			private set { SetValue<IModule>("Main", ref _Main, value); }
		}
		public bool IsVisible
		{
			get { return _IsVisible; }
			private set { SetValue<bool>("IsVisible", ref _IsVisible, value); }
		}
		#endregion //Define


		public virtual List<IModule> GetSubModules()
		{
			return new List<IModule>();
		}
		public virtual void BeginInit() { }
		public virtual void EndInit() { }
		/// <summary>
		/// 화면이 생성되기 전에 데이터를 전달
		/// </summary>
		/// <param name="parameter"></param>
		protected virtual void LoadData(object parameter) { }
		/// <summary>
		/// 화면이 표시된 후 데이터 갱신만 필요 한 경우 새로운 데이터를 전달
		/// </summary>
		/// <param name="parameter"></param>
		protected virtual void ReLoadData(object parameter) { }
		/// <summary>
		/// 화면이 생성 된 후 속성의 초기값을 지정(화면 갱신)
		/// </summary>
		/// <param name="parameter"></param>
		protected virtual void InitData(object parameter) { }
		/// <summary>
		/// ViewModel 속성 값에 대한 저장을 지원
		/// </summary>
		protected virtual void SaveData() { }
		/// <summary>
		/// 객체 Dispose
		/// </summary>
		protected override void DisposeManaged()
		{
			if (View != null) (View as BaseUserControl).Dispose();
			View = null;
			Main = null;
			Parent = null;

			if (BeforeDisappear != null) BeforeDisappear = null;
			if (BeforeAppearAsync != null) BeforeAppearAsync = null;
			if (BeforeAppear != null) BeforeAppear = null;
			if (UpdateDataContext != null) UpdateDataContext = null;

			base.DisposeManaged();
		}

		void IModule.SetView(object v)
		{
			View = v;
		}
		void IModule.SetIsVisible(bool v)
		{
			IsVisible = v;
		}
		void IModule.RaiseBeforeDisappear()
		{
			SaveData();
			if (BeforeDisappear != null)
				BeforeDisappear(this, EventArgs.Empty);
		}
		object IModule.InitParam
		{
			get { return initParam; }
			set { initParam = value; }
		}
		void IModule.RaiseBeforeAppearAsync()
		{
			LoadData(initParam);
			if (BeforeAppearAsync != null)
				BeforeAppearAsync(this, EventArgs.Empty);
		}
		void IModule.RaiseBeforeAppear()
		{
			InitData(initParam);
			if (BeforeAppear != null)
				BeforeAppear(this, EventArgs.Empty);
		}
		void IModule.SetModulesManager(IModulesManager v)
		{
			ModulesManager = v;
		}
		ModulesManagerInternalData IModule.ModulesManagerInternalData
		{
			get { return internalData; }
			set { internalData = value; }
		}
		void IModule.SetParent(IModule v)
		{
			Parent = v;
			Main = (v == null) ? this : v.Main;
		}
		/// <summary>
		/// DataContext Update
		/// </summary>
		/// <param name="parameter"></param>
		void IModule.RaiseUpdateDataContext(object parameter)
		{
			if (UpdateDataContext != null)
				UpdateDataContext(this, new SourceEventArgs(parameter));
		}
	}
}
