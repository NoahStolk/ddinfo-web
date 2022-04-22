using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Spawnsets;

public partial class Index : IHasNavigation
{
	[Parameter]
	[SupplyParameterFromQuery]
	public bool PracticeOnly { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public bool WithCustomLeaderboardOnly { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public string? SpawnsetFilter { get; set; }

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

	private Dictionary<SpawnsetSorting, bool> _sortings = new();

	public Page<GetSpawnsetOverview>? GetSpawnsets { get; set; }

	public int TotalPages => GetSpawnsets == null ? 0 : (GetSpawnsets.TotalResults - 1) / PagingUtils.GetValidPageSize(PageSize) + 1;
	public int TotalResults => GetSpawnsets == null ? 0 : GetSpawnsets.TotalResults;

	protected override void OnInitialized()
	{
		foreach (SpawnsetSorting e in (SpawnsetSorting[])Enum.GetValues(typeof(SpawnsetSorting)))
			_sortings.Add(e, false);
	}

	protected override async Task OnParametersSetAsync()
	{
		await Fetch();
	}

	private void ChangeInputSpawnsetName(ChangeEventArgs e)
	{
		SpawnsetFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(nameof(SpawnsetFilter), SpawnsetFilter);
	}

	private void ChangeInputAuthorName(ChangeEventArgs e)
	{
		AuthorFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(nameof(AuthorFilter), AuthorFilter);
	}

	private void ChangeInputPracticeOnly(bool value)
	{
		PracticeOnly = value;
		NavigationManager.AddOrModifyQueryParameter(nameof(PracticeOnly), PracticeOnly);
	}

	private void ChangeInputWithCustomLeaderboardOnly(bool value)
	{
		WithCustomLeaderboardOnly = value;
		NavigationManager.AddOrModifyQueryParameter(nameof(WithCustomLeaderboardOnly), WithCustomLeaderboardOnly);
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

	private void Sort(SpawnsetSorting sortBy)
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
		SpawnsetSorting sortBy = SortBy.HasValue ? (SpawnsetSorting)SortBy.Value : SpawnsetSorting.LastUpdated;

		GetSpawnsets = await Http.GetSpawnsets(PracticeOnly, WithCustomLeaderboardOnly, SpawnsetFilter, AuthorFilter, pageIndex, pageSize, sortBy, Ascending);

		if (PageIndex >= TotalPages)
		{
			PageIndex = TotalPages - 1;
			NavigationManager.AddOrModifyQueryParameter(nameof(PageIndex), PageIndex);
		}
	}
}
