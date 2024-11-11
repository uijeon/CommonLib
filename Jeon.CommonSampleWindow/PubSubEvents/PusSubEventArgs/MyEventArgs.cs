using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonSampleWindow.PubSubEvents.PusSubEventArgs
{
	public class MyEventArgs
	{
		public string Text { get; set; }

		public MyEventArgs(string text)
		{
			this.Text = text;
		}
	}
}
