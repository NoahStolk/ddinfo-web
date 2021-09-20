using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pagination
{
	public class PaginatedList<T> : List<T>
	{
		public PaginatedList(List<T> items, int pageIndex, int pageSize)
		{
			if (pageIndex < 1)
				throw new Exception("Page index cannot be 0 or negative.");

			PageIndex = pageIndex;
			TotalPages = (int)Math.Ceiling(items.Count / (double)pageSize);

			AddRange(items.Skip((pageIndex - 1) * pageSize).Take(pageSize));
		}

		public int PageIndex { get; }
		public int TotalPages { get; }

		public bool HasPreviousPage => PageIndex > 1;
		public bool HasNextPage => PageIndex < TotalPages;
	}
}
