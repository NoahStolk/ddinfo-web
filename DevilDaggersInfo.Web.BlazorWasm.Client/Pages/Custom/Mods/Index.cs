using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Mods;

public partial class Index : IHasNavigation
{
	[Parameter]
	[SupplyParameterFromQuery]
	public bool HostedOnly { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public string? ModFilter { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public string? AuthorFilter { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public int PageIndex { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public int PageSize { get; set; } = PagingConstants.PageSizeDefault;

	[Parameter]
	[SupplyParameterFromQuery]
	public int? SortBy { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public bool Ascending { get; set; }

	private Dictionary<ModSorting, bool> _sortings = new();

	public Page<GetModOverview>? GetMods { get; set; }

	public int TotalPages => GetMods == null ? 0 : (GetMods.TotalResults - 1) / PagingUtils.GetValidPageSize(PageSize) + 1;
	public int TotalResults => GetMods == null ? 0 : GetMods.TotalResults;

	protected override void OnInitialized()
	{
		foreach (ModSorting e in (ModSorting[])Enum.GetValues(typeof(ModSorting)))
			_sortings.Add(e, false);
	}

	protected override async Task OnParametersSetAsync()
	{
		await Fetch();
	}

	private void ChangeInputModName(ChangeEventArgs e)
	{
		ModFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(nameof(ModFilter), ModFilter);
	}

	private void ChangeInputAuthorName(ChangeEventArgs e)
	{
		AuthorFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(nameof(AuthorFilter), AuthorFilter);
	}

	private void ChangeInputHostedOnly(ChangeEventArgs e)
	{
		HostedOnly = bool.TryParse(e.Value?.ToString(), out bool value) && value;
		NavigationManager.AddOrModifyQueryParameter(nameof(HostedOnly), HostedOnly);
	}

	public async Task ChangePageIndex(int pageIndex)
	{
		PageIndex = Math.Clamp(pageIndex, 0, TotalPages - 1);
		NavigationManager.AddOrModifyQueryParameter(nameof(PageIndex), PageIndex);

		await Task.CompletedTask;
	}

	public async Task ChangePageSize(int pageSize)
	{
		PageSize = pageSize;
		NavigationManager.AddOrModifyQueryParameter(nameof(PageSize), PageSize);

		PageIndex = Math.Clamp(PageIndex, 0, TotalPages - 1);

		await Task.CompletedTask;
	}

	private void Sort(ModSorting sortBy)
	{
		SortBy = (int)sortBy;
		_sortings[sortBy] = !_sortings[sortBy];
		Ascending = _sortings[sortBy];

		NavigationManager.AddOrModifyQueryParameters(new(nameof(SortBy), SortBy), new(nameof(Ascending), Ascending));
	}

	private async Task Fetch()
	{
		int pageIndex = Math.Max(0, PageIndex);
		int pageSize = PagingUtils.GetValidPageSize(PageSize);
		ModSorting sortBy = SortBy.HasValue ? (ModSorting)SortBy.Value : ModSorting.LastUpdated;

		GetMods = await Http.GetMods(HostedOnly, ModFilter, AuthorFilter, pageIndex, pageSize, sortBy, Ascending);

		if (PageIndex >= TotalPages)
		{
			PageIndex = TotalPages - 1;
			NavigationManager.AddOrModifyQueryParameter(nameof(PageIndex), PageIndex);
		}
	}
}
