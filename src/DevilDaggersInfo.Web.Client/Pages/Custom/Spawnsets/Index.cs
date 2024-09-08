using DevilDaggersInfo.Web.ApiSpec.Main;
using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;
using DevilDaggersInfo.Web.Client.Extensions;
using DevilDaggersInfo.Web.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Pages.Custom.Spawnsets;

public partial class Index : IHasNavigation
{
	private readonly Dictionary<SpawnsetSorting, bool> _sortings = new();

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
	public int PageSize { get; set; } = Constants.PageSizeDefault;

	[Parameter]
	[SupplyParameterFromQuery]
	public int? SortBy { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public bool Ascending { get; set; }

	public Page<GetSpawnsetOverview>? GetSpawnsets { get; set; }

	public int TotalPages => GetSpawnsets == null ? 0 : (GetSpawnsets.TotalResults - 1) / PagingUtils.GetValidPageSize(PageSize) + 1;
	public int TotalResults => GetSpawnsets?.TotalResults ?? 0;

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

	private void ChangeInputWithCustomLeaderboardOnly(bool value)
	{
		WithCustomLeaderboardOnly = value;
		NavigationManager.AddOrModifyQueryParameter(nameof(WithCustomLeaderboardOnly), WithCustomLeaderboardOnly);
	}

	public async Task ChangePageIndexAsync(int pageIndex)
	{
		PageIndex = Math.Clamp(pageIndex, 0, TotalPages - 1);
		NavigationManager.AddOrModifyQueryParameter(nameof(PageIndex), PageIndex);

		await Task.CompletedTask;
	}

	public async Task ChangePageSizeAsync(int pageSize)
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

		NavigationManager.AddOrModifyQueryParameters(new KeyValuePair<string, object?>(nameof(SortBy), SortBy), new KeyValuePair<string, object?>(nameof(Ascending), Ascending));
	}

	private async Task Fetch()
	{
		int pageIndex = Math.Max(0, PageIndex);
		int pageSize = PagingUtils.GetValidPageSize(PageSize);
		SpawnsetSorting sortBy = SortBy.HasValue ? (SpawnsetSorting)SortBy.Value : SpawnsetSorting.LastUpdated;

		GetSpawnsets = await Http.GetSpawnsets(WithCustomLeaderboardOnly, SpawnsetFilter, AuthorFilter, pageIndex, pageSize, sortBy, Ascending);

		if (PageIndex >= TotalPages)
		{
			PageIndex = TotalPages - 1;
			NavigationManager.AddOrModifyQueryParameter(nameof(PageIndex), PageIndex);
		}
	}
}
