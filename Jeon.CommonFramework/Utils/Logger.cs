
namespace Jeon.CommonFramework.Utils
{
	using System;
	using System.Reflection;
	using log4net;
	using log4net.Config;

	public static class Logger
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		static Logger()
		{
			XmlConfigurator.Configure();
		}

		public static void WriteException(string logMessage, Exception ex = null)
		{
			var dataString = string.Empty;
			if (ex != null)
			{
				dataString += logMessage;
				dataString += $"\nError Message : {ex.Message}\n";
				if (null != ex.InnerException)
				{
					dataString += $"\nInnerException Message : {ex.InnerException.Message}\n";
				}
			}
			else
			{
				dataString += logMessage;
			}

			Log.Error(dataString, ex);
		}

		public static void WriteError(string logMessage)
		{
			Log.Error(logMessage);
		}

		public static void WriteError(string logMessage, Exception ex)
		{
			Log.Error(logMessage, ex);
		}

		public static void WriteInfo(string logMessage, Exception ex  = null)
		{
			Log.Info(logMessage, ex);
		}

		public static void WriteWarning(string logMessage, Exception ex = null)
		{
			Log.Warn(logMessage, ex);
		}

		public static void WriteDebug(string logMessage, Exception ex = null)
		{
			Log.Debug(logMessage, ex);
		}
	}
}
