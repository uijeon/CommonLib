using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Jeon.ViewModelFramework.Interface;
using Jeon.ViewModelFramework.RelayCommands;

namespace Jeon.ViewModelFramework.ViewModels
{
	/// <summary>
	/// Model, Type 을 등록할 수 있는 컬렉션을 가지는 MainContiner ViewModel
	/// </summary>
	public abstract class BaseMainControlViewModel : BaseControlViewModel
	{
		IModule currentModule;
		Type currentModuleType;
		ObservableCollection<BaseControlViewModel> modelCollection = new ObservableCollection<BaseControlViewModel>();



		public BaseMainControlViewModel()
		{
			IsPersistentModule = true;
			modelCollection.CollectionChanged += modelCollection_CollectionChanged;
		}


		void modelCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (BaseControlViewModel item in e.OldItems)
				{
					RaiseBeforeViewDisappear(item.View);
					RaiseAfterViewDisappear(item.View);
					item.Dispose();
				}
				NotifyPropertyChanged("ModelCollection");
			}
			else if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (BaseControlViewModel item in e.NewItems)
				{
					NotifyPropertyChanged("ModelCollection");
				}
			}
		}

		public override void EndInit()
		{
			base.EndInit();
			CurrentModuleType = GetType();
			CurrentModule = this;
		}

		protected override void DisposeManaged()
		{
			base.DisposeManaged();

			foreach (var model in ModelCollection)
				model.Dispose();
			ModelCollection.CollectionChanged -= modelCollection_CollectionChanged;
			ModelCollection.Clear();
			ModelCollection = null;
		}

		/// <summary>
		/// 표시하려는 모델을 생성(등록)하고 데이터 전달
		/// </summary>
		/// <typeparam name="TModule"></typeparam>
		/// <param name="parameter"></param>
		public void ShowModule<TModule>(object parameter) where TModule : class, IModule, new()
		{
			CurrentModuleType = typeof(TModule);
			CurrentModule = ModulesManager.CreateModule<TModule>(null, this, parameter);
			if (!ModelCollection.Contains(CurrentModule))
				ModelCollection.Add((BaseControlViewModel)CurrentModule);
		}
		/// <summary>
		/// 표시하려는 모델을 생성하고 데이터 전달
		/// </summary>
		/// <typeparam name="TModule"></typeparam>
		/// <param name="parameter"></param>
		public void ShowModule(Type moduleType, object parameter)
		{
			if (moduleType == null) return;
			CurrentModuleType = moduleType;
			CurrentModule = ModulesManager.CreateModule(moduleType, null, this, parameter);
		}

		/// <summary>
		/// Current Model Type
		/// </summary>
		public Type CurrentModuleType
		{
			get { return currentModuleType; }
			set { SetValue<Type>("CurrentModuleType", ref currentModuleType, value); }
		}
		/// <summary>
		/// Current Model 객체(BaseControlViewModel)
		/// </summary>
		public IModule CurrentModule
		{
			get { return currentModule; }
			set { SetValue<IModule>("CurrentModule", ref currentModule, value); }
		}
		/// <summary>
		/// Model 객체 컬렉션
		/// </summary>
		public ObservableCollection<BaseControlViewModel> ModelCollection
		{
			get { return modelCollection; }
			set { SetValue<ObservableCollection<BaseControlViewModel>>("ModelCollection", ref modelCollection, value); }
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

		/// <summary>
		/// 모델 생성 Command
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		protected virtual ICommand CreateShowModuleCommand<T>() where T : class, IModule, new()
		{
			return new ExtendedActionCommand(p => ShowModule<T>(p), this, "ModelCollection", x => ModelCollection.OfType<T>().Count() == 0, null);
		}
		/// <summary>
		/// 모델 생성 Command
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="param">RoleBack Func 에 전달할 데이터</param>
		/// <returns></returns>
		protected virtual ICommand CreateShowModuleCommand<T>(object param) where T : class, IModule, new()
		{
			return new ExtendedActionCommand(p => ShowModule<T>(p), this, "ModelCollection", x => ModelCollection.OfType<T>().Count() == 0, param);
		}
	}
}
