using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Jeon.CommonFramework.Timers
{
	/// <summary>
	/// UI Thread 에서 동작하는 타이머
	/// </summary>
	internal class DispatcherTimer : BaseTimer
	{
		private readonly SynchronizationContext _syncContext = SynchronizationContext.Current;

		private Timer _publiserTimer = new Timer();

		private int _Interval = 1000;

		public override int Interval
		{
			get
			{
				return this._Interval;
			}
			set
			{
				this._Interval = value;
				this._publiserTimer.Interval = value;
			}
		}

		public override event Action Tick;

		public DispatcherTimer(int interval)
		{
			this.Interval = interval;

			this.Init();
		}

		private void Init()
		{
			this._publiserTimer.Elapsed += (o, e) => { this.RaisedTimerTick(); };
		}

		public override void Start()
		{
			if (this._syncContext == null)
			{
				throw new InvalidOperationException("The DispatcherTimerWrapper must be created on the UI thread.");
			}

			this._publiserTimer.Start();
		}

		public override void Stop()
		{
			this._publiserTimer.Stop();
		}

		protected override void RaisedTimerTick()
		{
			this._syncContext.Post(o => this.Tick(), null);
		}

		public override void Dispose()
		{
			if (this._publiserTimer == null)
			{
				return;
			}

			this._publiserTimer.Stop();
			this._publiserTimer.Dispose();
			this._publiserTimer = null;
		}
	}
}
