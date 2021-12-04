namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom;

public interface ICustomPage
{
	public int PageIndex { get; set; }
	public int PageSize { get; set; }

	public int TotalPages { get; }

	public Task ChangePageIndex(int pageIndex);
}
