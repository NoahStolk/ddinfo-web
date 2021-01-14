using DevilDaggersWebsite.Razor.Pagination;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public interface IPaginationModel
	{
		public IPaginatedList? PaginatedList { get; }

		public string PageName { get; }
		public string? SearchAuthor { get; set; }
		public string? SearchName { get; set; }

		public string? SortOrder { get; set; }
		public int PageSize { get; set; }
		public int PageIndex { get; }
		public int TotalResults { get; }
	}
}
