using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Shared
{
	public class ArrayData
	{
		public double Average { get; private set; }
		public int Median { get; private set; }
		public int Mode { get; private set; }

		public void Populate(IEnumerable<int> data)
		{
			data = data.OrderBy(n => n);

			Average = data.Average();
			Median = data.ElementAt(data.Count() / 2);
			Mode = data
				.GroupBy(n => n)
				.OrderByDescending(g => g.Count())
				.Select(g => g.Key)
				.FirstOrDefault();
		}
	}
}
