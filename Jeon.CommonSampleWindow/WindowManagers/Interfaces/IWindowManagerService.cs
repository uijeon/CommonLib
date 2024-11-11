using Jeon.CommonSampleWindow.WindowManagers.Enums;
using System.Windows;

namespace Jeon.CommonSampleWindow.WindowManagers.Interfaces
{
	public interface IWindowManagerService
	{
		/// <summary>
		/// CommonWindow를 이용한 Window 호출 시 사용.
		/// </summary>
		/// <param name="windowContent">대상 Window 가 사용할 Content 객체.(ViewModel)</param>
		/// <param name="setting">Window 초기 형태를 결정할 Setting type.</param>
		/// <param name="ownerViewModel">Owner로 사용할 Window의 ViewModel.</param>
		/// <param name="isCreate">true 이면 새로 생성; false 이면 WindowManagerService에 캐쉬된 부분 사용.</param>
		/// <returns></returns>
		IWindowView GetOrCreateWindow(IWindowContent windowContent, WindowSettings setting, IWindowContent ownerViewModel = null, bool isCreate = false);

		/// <summary>
		/// CommonWindow를 이용한 Window 호출 시 사용.
		/// </summary>
		/// <param name="windowContent">대상 Window 가 사용할 Content 객체.(ViewModel)</param>
		/// <param name="setting">Window 초기 형태를 결정할 Setting instance.</param>
		/// <param name="ownerViewModel">Owner로 사용할 Window의 ViewModel.</param>
		/// <param name="isCreate">true 이면 새로 생성; false 이면 WindowManagerService에 캐쉬된 부분 사용.</param>
		/// <returns></returns>
		IWindowView GetOrCreateWindow(IWindowContent windowContent, IWindowSetting setting, IWindowContent ownerViewModel = null, bool isCreate = false);

		/// <summary>
		/// CustomWindow를 이용한 Window 호출 시 사용.
		/// </summary>
		/// <typeparam name="T">CustomWindow Type</typeparam>
		/// <param name="windowContent">대상 Window 가 사용할 Content 객체.(ViewModel)</param>
		/// <param name="setting">Window 초기 형태를 결정할 Setting instance.</param>
		/// <param name="ownerViewModel">Owner로 사용할 Window의 ViewModel.</param>
		/// <param name="isCreate">true 이면 새로 생성; false 이면 WindowManagerService에 캐쉬된 부분 사용.</param>
		/// <returns></returns>
		IWindowView GetOrCreateWindow<T>(IWindowContent windowContent, IWindowSetting setting, IWindowContent ownerViewModel = null, bool isCreate = false);

		void SetOwner(IWindowContent targetViewModel, IWindowContent ownerViewModel);

		void SetOwner(IWindowView targetView, IWindowView ownerView);

		void ChangeWindowState(IWindowContent windowViewModel, WindowState states);

		void ChangeWindowState(IWindowView windowView, WindowState states);

		void DragMove(IWindowContent windowViewModel);

		void DragMove(IWindowView windowView);

		void CloseWindow(IWindowContent windowViewModel);

		void HideWindow(IWindowContent windowViewModel);

		void SetDialogResult(IWindowContent windowViewModel, bool? value);

		void CloseWindow(IWindowView windowView);

		void Release();
	}
}
