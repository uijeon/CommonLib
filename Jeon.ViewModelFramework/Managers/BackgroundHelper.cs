using System;
using System.Threading;
using System.Windows.Threading;

namespace Jeon.ViewModelFramework.Managers
{
	/// <summary>
	/// Main UI Thread 에 적용 될 수 있도록 도와줌
	/// </summary>
	public static class BackgroundHelper
	{
		public static void DoInBackground(Action backgroundAction, Action mainThreadAction)
		{
			DoInBackground(backgroundAction, mainThreadAction, 200);
		}
		public static void DoInBackground(Action backgroundAction, Action mainThreadAction, int milliseconds)
		{
			try
			{
#if SL
			    Dispatcher dispatcher = Dispatcher;
#else
				Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
#endif
				Thread thread = new Thread(delegate ()
				{
					Thread.Sleep(milliseconds);
					if (backgroundAction != null)
						backgroundAction();
					if (mainThreadAction != null)
						dispatcher.BeginInvoke(mainThreadAction);
				});
				thread.IsBackground = true;
				thread.TrySetApartmentState(ApartmentState.STA);
#if !SL
				thread.Priority = ThreadPriority.Lowest;
#endif
				thread.Start();
			}
			catch (Exception)
			{
			}
		}
		public static void DoWithDispatcher(Dispatcher dispatcher, Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
		{
			try
			{
				if (dispatcher.CheckAccess())
					action();
				else
				{
					AutoResetEvent done = new AutoResetEvent(false);
					dispatcher.BeginInvoke((Action)delegate ()
					{
						action();
						done.Set();
					}, dispatcherPriority);
					done.WaitOne();
				}
			}
			catch (Exception)
			{
			}
		}
#if SL
		public static Dispatcher Dispatcher {
			get {
				return Deployment.Current.Dispatcher;
			}
		}
#endif
	}
}
