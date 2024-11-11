using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeon.CommonFramework.Timers
{
	/// <summary>
	/// 비동기 Thread 타이머, 자체 쓰레드에서 동작
	/// </summary>
	internal class BackgroundTimer : BaseTimer
	{
		private ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

		private Thread _backgroundTimerThread;

		public override event Action Tick;

		public override int Interval { get; set; } = 1000;

		public BackgroundTimer(int interval)
		{
			this.Interval = interval;

			this.Init();
		}

		private void Init()
		{
			this._backgroundTimerThread = new Thread(new ThreadStart(this.RaisedTimerTick));
			this._backgroundTimerThread.IsBackground = true;
			this._backgroundTimerThread.Priority = ThreadPriority.Normal;
		}

		public override void Start()
		{
			this._manualResetEvent.Set();

			if (this._backgroundTimerThread.ThreadState == (ThreadState.Unstarted | ThreadState.Background))
			{
				this._backgroundTimerThread.Start();
			}
		}

		public override void Stop()
		{
			this._manualResetEvent.Reset();
		}

		protected override void RaisedTimerTick()
		{
			while (true)
			{
				Thread.Sleep(this.Interval);

				this._manualResetEvent.WaitOne();

				this.Tick();
			}
		}

		public override void Dispose()
		{
			if (this._backgroundTimerThread == null)
			{
				return;
			}

			if (this._backgroundTimerThread.ThreadState == (ThreadState.Unstarted | ThreadState.Background))
			{
				this._backgroundTimerThread.Join();
			}

			this._backgroundTimerThread = null;
		}
	}
}