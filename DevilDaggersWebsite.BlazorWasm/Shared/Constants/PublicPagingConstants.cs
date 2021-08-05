using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Constants
{
	public static class PublicPagingConstants
	{
		public const int PageSizeDefault = 25;
		public const int PageSizeMin = 15;
		public const int PageSizeMax = 35;

		public static IEnumerable<int> PageSizeOptions { get; } = Enumerable.Range(3, 4).Select(i => i * 5);
	}
}
