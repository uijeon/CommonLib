namespace Jeon.CommonFramework.EventAggregatorParts
{
	public interface IEventAggregator
	{
		TEventType GetEvent<TEventType>() where TEventType : EventBase, new();
	}
}
