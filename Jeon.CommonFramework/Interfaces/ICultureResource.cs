using System.Globalization;
using System.Windows.Data;

namespace Jeon.CommonFramework.Interfaces
{
	public interface ICultureResource
	{
		ObjectDataProvider ResourceProvider { get; }

		void ChangeCulture(CultureInfo culture);
	}
}
