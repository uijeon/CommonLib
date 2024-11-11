using Jeon.CommonFramework.EventAggregatorParts;
using N3N.Service.ServiceLocator;
using Jeon.CommonSampleWindow.WindowManagers.Interfaces;
using Jeon.ViewModelFramework.BaseNotify;

namespace Jeon.CommonSampleWindow.ViewModels.Bases
{
	public abstract class BaseViewModel : ObservableObject
	{
		#region Fields

		/// <summary>
		/// 내부 사용을 위한 EventAggregator 필드. 직접 사용 보다 EventAggregator property 이용할 것.
		/// </summary>
		private IEventAggregator _eventAggregator;

		/// <summary>
		/// 내부 사용을 위한 WindowManagerService 필드. 직접 사용 보다 WindowManagerService property 이용할 것.
		/// </summary>
		private IWindowManagerService _windowManagerService;

		#endregion

		#region Properties

		/// <summary>
		/// 내부 사용을 위한 EventAggregator 초기화 객체.
		/// </summary>
		protected IEventAggregator EventAggregator
		{
			get
			{
				this.InitEventAggregator();

				return this._eventAggregator;
			}
		}

		/// <summary>
		/// 내부 사용을 위한 WindowManagerService 초기화 객체.
		/// </summary>
		protected IWindowManagerService WindowManagerService
		{
			get
			{
				this.InitWindowManagerService();

				return this._windowManagerService;
			}
		}

		#endregion

		#region Methods

		private void InitEventAggregator()
		{
			if (this._eventAggregator != null)
			{
				return;
			}

			// TODO : 이하 구문 실패 시 예외 발생 처리 필요.
			var container = ContainerResolver.GetContainer();
			this._eventAggregator = container.Resolve<IEventAggregator>();
		}

		private void InitWindowManagerService()
		{
			if (this._windowManagerService != null)
			{
				return;
			}

			// TODO : 이하 구문 실패 시 예외 발생 처리 필요.
			var container = ContainerResolver.GetContainer();
			this._windowManagerService = container.Resolve<IWindowManagerService>();
		}

		#endregion
	}
}
