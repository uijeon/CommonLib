using System.Windows;
using System.Windows.Media;

namespace Jeon.ViewFramework.Utils
{
	public static class VisualHelper
	{
		/// <summary>
		/// child에서 T 타입의 부모 Element 를 찾는다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="child"></param>
		/// <returns></returns>
		public static T GetVisualAncestor<T>(DependencyObject child) where T : DependencyObject
		{
			//get parent item
			var parentObject = VisualTreeHelper.GetParent(child);

			//we've reached the end of the tree
			if (parentObject == null)
			{
				return null;
			}

			//check if the parent matches the type we're looking for
			T parent = parentObject as T;

			if (parent != null)
			{
				return parent;
			}

			return GetVisualAncestor<T>(parentObject);
		}
	}
}
