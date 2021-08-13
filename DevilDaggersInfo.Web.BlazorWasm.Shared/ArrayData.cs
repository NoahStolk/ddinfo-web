using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared
{
	public class ArrayData
	{
		// Make setters public so this can be used as DTO.
		public double Average { get; set; }
		public int Median { get; set; }
		public int Mode { get; set; }

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
