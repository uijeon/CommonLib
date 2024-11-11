using Jeon.CommonFramework.EventAggregatorParts;
using Jeon.CommonSampleWindow.PubSubEvents;
using Jeon.CommonSampleWindow.PubSubEvents.PusSubEventArgs;
using Jeon.CommonSampleWindow.ViewModels.Bases;
using Jeon.CommonSampleWindow.WindowManagers.Interfaces;

namespace Jeon.CommonSampleWindow.ViewModels
{
    public class EventPopupViewModel : BaseViewModel, IWindowContent
	{
		public EventPopupViewModel()
		{
			this.EventAggregator.GetEvent<NotArgumentEvent>().Subscribe(this.SetNotArgument, ThreadOption.UIThread);
			this.EventAggregator.GetEvent<ArgumentEvent>().Subscribe(this.SetArgument, ThreadOption.UIThread);
		}
		public string MainText { get; set; }

		private void SetNotArgument()
		{
			this.MainText = "인수 없는 이벤트";
		}

		public void SetArgument(MyEventArgs arg)
		{
			this.MainText = "인수 있는 이벤트" + "\n" + arg.Text;
		}

		public void Close() { }
	}
}
