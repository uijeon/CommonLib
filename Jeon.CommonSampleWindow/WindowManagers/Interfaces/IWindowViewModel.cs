using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonSampleWindow.WindowManagers.Interfaces
{
	public interface IWindowViewModel : IWindowSetting
	{
		IWindowContent ViewContext { get; set; }
	}
}
