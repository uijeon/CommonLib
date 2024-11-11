namespace Jeon.CommonSampleWindow.WindowManagers.Interfaces
{
	public interface IWindowContent
	{
		// TODO : 하위 View에서 필요한 기능이 있을 경우 추가 작성 필요.

		/// <summary>
		/// Close 시 하위 View 에서 전 처리할 내용이 있으면 작성
		/// </summary>
		void Close();
	}
}
