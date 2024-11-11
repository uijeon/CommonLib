using Jeon.CommonFramework.ExtensionMethods;
using Jeon.CommonSampleWindow.WindowManagers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Jeon.CommonSampleWindow.Windows
{
	public abstract class BaseWindow : Window, IWindowView
	{
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			this.IsClosed = true;
		}

		public void SetOwner(IWindowView windowView)
		{
			if (windowView is Window == false)
			{
				return;
			}

			var ownerWindow = windowView as Window;
			this.Owner = ownerWindow;
		}

		public void MoveWindow()
		{
			this.DragMove();
		}

		public void WindowStateChange(WindowState states)
		{
			this.WindowState = states.GetEnum<WindowState>();
		}

		public bool IsClosed { get; set; }
	}
}
