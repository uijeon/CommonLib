using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonFramework.ExtensionMethods
{
	public static class ConcurrentQueueExtensions
	{
		public static void Clear<T>(this ConcurrentQueue<T> queue)
		{
			T item;
			while (queue.TryDequeue(out item));
		}
	}
}
