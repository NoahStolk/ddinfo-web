using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Shared
{
	public struct ArrayData
	{
		public ArrayData(IEnumerable<int> data)
		{
			data = data.OrderBy(n => n);

			Total = data.Count();
			Average = data.Average();
			Median = data.ElementAt(Total / 2);
			Mode = data
				.GroupBy(n => n)
				.OrderByDescending(g => g.Count())
				.Select(g => g.Key)
				.FirstOrDefault();
		}

		public int Total { get; }
		public double Average { get; }
		public int Median { get; }
		public int Mode { get; }
	}
}
