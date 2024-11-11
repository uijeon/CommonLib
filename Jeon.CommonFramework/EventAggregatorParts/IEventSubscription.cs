using System;

namespace Jeon.CommonFramework.EventAggregatorParts
{
	public interface IEventSubscription
	{
		SubscriptionToken SubscriptionToken { get; set; }

		Action<object[]> GetExecutionStrategy();
	}
}
