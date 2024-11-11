using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonFramework.ExtensionMethods
{
	public static class EnumExtension
	{
		public static T GetEnum<T>(this object target) where T : struct
		{
			// TODO : need validtion check.

			var type = typeof(T);

			if (type != typeof(int) || type != typeof(string))
			{
				return default(T);
			}

			return (T)Enum.Parse(typeof(T), target.ToString(), true);
		}
	}
}
