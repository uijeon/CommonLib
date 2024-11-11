using System;
using Timer = System.Timers.Timer;

namespace Jeon.CommonFramework.Timers
{
	/// <summary>
	/// 기본적으로 제공하는 타이머, Thread Pool 에서 동작한다
	/// </summary>
	internal class PublisherTimer : BaseTimer
	{
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

		public PublisherTimer(int interval)
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
			this._publiserTimer.Start();
		}

		public override void Stop()
		{
			this._publiserTimer.Stop();
		}

		protected override void RaisedTimerTick()
		{
			this.Tick();
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