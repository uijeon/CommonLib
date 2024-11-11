using System;

namespace Jeon.CommonFramework.EventAggregatorParts
{
	public interface IDelegateReference
	{
		Delegate Target { get; }
	}
}
