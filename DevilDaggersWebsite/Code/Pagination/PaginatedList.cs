using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Pagination
{
	public class PaginatedList<T> : List<T>
	{
		public int PageIndex { get; private set; }
		public int TotalPages { get; private set; }

		public bool HasPreviousPage => PageIndex > 1;
		public bool HasNextPage => PageIndex < TotalPages;

		public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
		{
			PageIndex = pageIndex;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);

			AddRange(items);
		}

		public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
		{
			if (pageIndex < 1)
				throw new Exception("Page index cannot be 0 or negative.");

			int count = await source.CountAsync();
			List<T> items = await source
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			return new PaginatedList<T>(items, count, pageIndex, pageSize);
		}

		public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
		{
			if (pageIndex < 1)
				throw new Exception("Page index cannot be 0 or negative.");

			int count = source.Count();
			List<T> items = source
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			return new PaginatedList<T>(items, count, pageIndex, pageSize);
		}
	}
}