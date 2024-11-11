using Jeon.CommonFramework.EventAggregatorParts;
using System;

namespace Jeon.CommonFramework.Timers
{
	/// <summary>
	/// 모든 타이머 사용을 위한 Wrapper 클래스
	/// 타이머 별로 따로 생성하는게 아닌 해당 클래스에서 Option 처리를 통해 Timer Type 을 정할 수 있도록 함.
	/// </summary>
	public class TimerWrapper : IDisposable
	{
		private BaseTimer _timerWrapper;

		private Action _action;

		private int _interval;

		private ThreadOption _threadOption { get; set; } = ThreadOption.PublisherThread;

		public TimerWrapper(int interval, Action action) : this(interval, action, ThreadOption.PublisherThread)
		{

		}

		public TimerWrapper(int interval, Action action, ThreadOption option)
		{
			this._threadOption = option;
			this._interval = interval;
			this._action = action;

			this.Init();
		}

		~TimerWrapper()
		{
			this.Dispose();
		}

		private void Init()
		{
			switch (this._threadOption)
			{
				case ThreadOption.UIThread:
					this._timerWrapper = new DispatcherTimer(this._interval);
					break;
				case ThreadOption.PublisherThread:
					this._timerWrapper = new PublisherTimer(this._interval);
					break;

				case ThreadOption.BackgroundThread:
					this._timerWrapper = new BackgroundTimer(this._interval);
					break;
			}

			if (this._action == null)
			{
				return;
			}

			this._timerWrapper.Tick += this._action;
		}

		public void Start()
		{
			this._timerWrapper.Start();
		}

		public void Stop()
		{
			this._timerWrapper.Stop();
		}

		public void Dispose()
		{
			if (this._timerWrapper == null)
			{
				return;
			}

			this._timerWrapper.Dispose();
			this._timerWrapper = null;
		}
	}
}
