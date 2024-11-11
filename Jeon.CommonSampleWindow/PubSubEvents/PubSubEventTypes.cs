using Jeon.CommonFramework.EventAggregatorParts;
using Jeon.CommonSampleWindow.PubSubEvents.PusSubEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonSampleWindow.PubSubEvents
{
	public class NotArgumentEvent : PubSubEvent { }

	public class ArgumentEvent : PubSubEvent<MyEventArgs> { }
}
