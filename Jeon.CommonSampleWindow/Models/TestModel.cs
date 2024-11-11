using System.Collections.Generic;

namespace Jeon.CommonSampleWindow.Models
{
	public class TestModel
	{
		public string Title { get; set; }

		public SecondTestModel Second { get; set; } = new SecondTestModel();

		public List<int> NumberList { get; set; } = new List<int>();
	}

	public class SecondTestModel
	{
		public int Number { get; set; } = 1;
	}
}
