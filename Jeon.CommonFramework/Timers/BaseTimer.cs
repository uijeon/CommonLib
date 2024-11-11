using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonFramework.Timers
{
	public abstract class BaseTimer : IDisposable
	{
		public abstract int Interval { get; set; }

		public abstract event Action Tick;

		public abstract void Start();

		public abstract void Stop();

		public abstract void Dispose();

		protected abstract void RaisedTimerTick();
	}
}
