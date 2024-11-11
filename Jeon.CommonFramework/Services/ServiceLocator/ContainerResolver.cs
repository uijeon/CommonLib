using Jeon.CommonFramework.Interfaces;
using Jeon.CommonFramework.ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N3N.Service.ServiceLocator
{
	public class ContainerResolver
	{
		private static readonly Lazy<ComponentContainer> LazyComponentContainer = new Lazy<ComponentContainer>(() => new ComponentContainer());

		public static IComponentContainer GetContainer()
		{
			return LazyComponentContainer.Value;
		}
	}
}
