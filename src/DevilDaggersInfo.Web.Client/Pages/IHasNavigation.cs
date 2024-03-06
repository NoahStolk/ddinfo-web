namespace DevilDaggersInfo.Web.Client.Pages;

public interface IHasNavigation
{
	public int PageIndex { get; set; }
	public int PageSize { get; set; }

	public int TotalPages { get; }
	public int TotalResults { get; }

	public Task ChangePageIndexAsync(int pageIndex);

	public Task ChangePageSizeAsync(int pageSize);
}
