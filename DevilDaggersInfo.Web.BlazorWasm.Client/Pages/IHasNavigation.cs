using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages;

public interface IHasNavigation
{
	public int PageIndex { get; set; }
	public int PageSize { get; set; }

	public int TotalPages { get; }

	public Task ChangePageIndex(int pageIndex);

	public Task ChangePageSize(ChangeEventArgs e);
}
