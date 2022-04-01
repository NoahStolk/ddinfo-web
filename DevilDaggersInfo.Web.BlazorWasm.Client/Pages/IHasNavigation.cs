using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages;

public interface IHasNavigation
{
	public int PageIndex { get; set; }
	public int PageSize { get; set; }

	public int TotalPages { get; }
	public int TotalResults { get; }

	public void ChangePageIndex(int pageIndex);

	public void ChangePageSize(int pageSize);
}
