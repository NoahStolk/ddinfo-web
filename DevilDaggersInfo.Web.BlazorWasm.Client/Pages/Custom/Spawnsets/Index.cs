using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Spawnsets;

public partial class Index
{
	private int _pageIndex;
	private int _pageSize = PagingConstants.PageSizeDefault;

	[Parameter, SupplyParameterFromQuery] public bool PracticeOnly { get; set; }
	[Parameter, SupplyParameterFromQuery] public bool WithCustomLeaderboardOnly { get; set; }
	[Parameter, SupplyParameterFromQuery] public string? SpawnsetFilter { get; set; }
	[Parameter, SupplyParameterFromQuery] public string? AuthorFilter { get; set; }
	[Parameter, SupplyParameterFromQuery] public int PageIndex { get => _pageIndex; set => _pageIndex = Math.Max(0, value); }
	[Parameter, SupplyParameterFromQuery] public int PageSize { get => _pageSize; set => _pageSize = value < PagingConstants.PageSizeMin || value > PagingConstants.PageSizeMax ? PagingConstants.PageSizeDefault : value; }
	[Parameter, SupplyParameterFromQuery] public int? SortBy { get; set; }
	[Parameter, SupplyParameterFromQuery] public bool Ascending { get; set; }

	private Dictionary<SpawnsetSorting, bool> _sortings = new();

	public Page<GetSpawnsetOverview>? GetSpawnsets { get; set; }

	public int TotalPages => GetSpawnsets == null ? 0 : (GetSpawnsets.TotalResults - 1) / PageSize + 1;
	public int TotalResults => GetSpawnsets == null ? 0 : GetSpawnsets.TotalResults;

	private async Task ChangeInputSpawnsetName(ChangeEventArgs e)
	{
		SpawnsetFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.SpawnsetFilter, SpawnsetFilter);

		await Fetch();
	}

	private async Task ChangeInputAuthorName(ChangeEventArgs e)
	{
		AuthorFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.AuthorFilter, AuthorFilter);

		await Fetch();
	}

	private async Task ChangeInputPracticeOnly(ChangeEventArgs e)
	{
		PracticeOnly = bool.TryParse(e.Value?.ToString(), out bool value) && value;
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.PracticeOnly, PracticeOnly);

		await Fetch();
	}

	private async Task ChangeInputWithCustomLeaderboardOnly(ChangeEventArgs e)
	{
		WithCustomLeaderboardOnly = bool.TryParse(e.Value?.ToString(), out bool value) && value;
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.WithCustomLeaderboardOnly, WithCustomLeaderboardOnly);

		await Fetch();
	}

	protected override async Task OnInitializedAsync()
	{
		foreach (SpawnsetSorting e in (SpawnsetSorting[])Enum.GetValues(typeof(SpawnsetSorting)))
			_sortings.Add(e, false);

		await Fetch();
	}

	public async Task ChangePageIndex(int pageIndex)
	{
		PageIndex = Math.Clamp(pageIndex, 0, TotalPages - 1);
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.PageIndex, PageIndex);

		await Fetch();

		StateHasChanged();
	}

	public async Task ChangePageSize(ChangeEventArgs e)
	{
		if (!int.TryParse(e.Value?.ToString(), out int pageSize))
			return;

		PageSize = pageSize;
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.PageSize, PageSize);

		PageIndex = Math.Clamp(PageIndex, 0, TotalPages - 1);
		await Fetch();
	}

	private async Task Sort(SpawnsetSorting sortBy)
	{
		SortBy = (int)sortBy;
		_sortings[sortBy] = !_sortings[sortBy];
		Ascending = _sortings[sortBy];

		NavigationManager.AddOrModifyQueryParameters(new(QueryParameters.SortBy, SortBy), new(QueryParameters.Ascending, Ascending));

		await Fetch();
	}

	private async Task Fetch()
	{
		GetSpawnsets = await Http.GetSpawnsets(PracticeOnly, WithCustomLeaderboardOnly, SpawnsetFilter, AuthorFilter, PageIndex, PageSize, SortBy.HasValue ? (SpawnsetSorting)SortBy.Value : SpawnsetSorting.LastUpdated, Ascending);

		if (PageIndex >= TotalPages)
		{
			PageIndex = TotalPages - 1;
			NavigationManager.AddOrModifyQueryParameter(QueryParameters.PageIndex, PageIndex);
		}
	}

	private static class QueryParameters
	{
		public static string SpawnsetFilter { get; } = nameof(SpawnsetFilter);
		public static string AuthorFilter { get; } = nameof(AuthorFilter);
		public static string PracticeOnly { get; } = nameof(PracticeOnly);
		public static string WithCustomLeaderboardOnly { get; } = nameof(WithCustomLeaderboardOnly);
		public static string PageIndex { get; } = nameof(PageIndex);
		public static string PageSize { get; } = nameof(PageSize);
		public static string SortBy { get; } = nameof(SortBy);
		public static string Ascending { get; } = nameof(Ascending);
	}
}
