using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonSampleWindow.WindowManagers.Enums
{
	public enum WindowSettings
	{
		Main,
		Popup
		// TODO : 필요 시 추가.
	}

	public enum MessageBoxTypes
	{
		Yes,
		YesNo,
		YesNoCancel,
		Ok,
		OkCancel
	}

	public enum VisibilityTypes
	{
		Visible,
		Collapsed,
		Hidden
	}
}
