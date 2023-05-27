namespace DevilDaggersInfo.Web.Client.Pages;

public interface IHasNavigation
{
	public int PageIndex { get; set; }
	public int PageSize { get; set; }

	public int TotalPages { get; }
	public int TotalResults { get; }

	public Task ChangePageIndex(int pageIndex);

	public Task ChangePageSize(int pageSize);
}
