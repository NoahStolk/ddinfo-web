using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Leaderboards;

public partial class Index
{
	private int _category = 1;
	private int _pageIndex;
	private int _pageSize = PagingConstants.PageSizeDefault;

	[Parameter, SupplyParameterFromQuery] public int Category { get => _category; set => _category = Math.Max(1, value); }
	[Parameter, SupplyParameterFromQuery] public string? SpawnsetFilter { get; set; }
	[Parameter, SupplyParameterFromQuery] public string? AuthorFilter { get; set; }
	[Parameter, SupplyParameterFromQuery] public int PageIndex { get => _pageIndex; set => _pageIndex = Math.Max(0, value); }
	[Parameter, SupplyParameterFromQuery] public int PageSize { get => _pageSize; set => _pageSize = value < PagingConstants.PageSizeMin || value > PagingConstants.PageSizeMax ? PagingConstants.PageSizeDefault : value; }
	[Parameter, SupplyParameterFromQuery] public int? SortBy { get; set; }
	[Parameter, SupplyParameterFromQuery] public bool Ascending { get; set; }

	private Dictionary<CustomLeaderboardSorting, bool> _sortings = new();

	public Page<GetCustomLeaderboardOverview>? GetCustomLeaderboards { get; set; }

	public GetTotalCustomLeaderboardData? GetTotalCustomLeaderboardData { get; set; }

	public int TotalPages => GetCustomLeaderboards == null ? 0 : (GetCustomLeaderboards.TotalResults - 1) / PageSize + 1;
	public int TotalResults => GetCustomLeaderboards == null ? 0 : GetCustomLeaderboards.TotalResults;

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

	protected override async Task OnInitializedAsync()
	{
		foreach (CustomLeaderboardSorting e in (CustomLeaderboardSorting[])Enum.GetValues(typeof(CustomLeaderboardSorting)))
			_sortings.Add(e, false);

		await Fetch();

		GetTotalCustomLeaderboardData = await Http.GetTotalCustomLeaderboardData();
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

	private async Task ChangeCategory(ChangeEventArgs e)
	{
		if (!Enum.TryParse<CustomLeaderboardCategory>(e.Value?.ToString(), out CustomLeaderboardCategory category))
			return;

		Category = (int)category;
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.Category, Category);

		await Fetch();
	}

	private async Task Sort(CustomLeaderboardSorting sortBy)
	{
		SortBy = (int)sortBy;
		_sortings[sortBy] = !_sortings[sortBy];
		Ascending = _sortings[sortBy];

		NavigationManager.AddOrModifyQueryParameters(new(QueryParameters.SortBy, SortBy), new(QueryParameters.Ascending, Ascending));

		await Fetch();
	}

	private async Task Fetch()
	{
		GetCustomLeaderboards = await Http.GetCustomLeaderboards((CustomLeaderboardCategory)Category, SpawnsetFilter, AuthorFilter, PageIndex, PageSize, SortBy.HasValue ? (CustomLeaderboardSorting)SortBy.Value : CustomLeaderboardSorting.DateLastPlayed, Ascending);

		if (PageIndex >= TotalPages)
		{
			PageIndex = TotalPages - 1;
			NavigationManager.AddOrModifyQueryParameter(QueryParameters.PageIndex, PageIndex);
		}
	}

	private static class QueryParameters
	{
		public static string Category { get; } = nameof(Category);
		public static string SpawnsetFilter { get; } = nameof(SpawnsetFilter);
		public static string AuthorFilter { get; } = nameof(AuthorFilter);
		public static string PageIndex { get; } = nameof(PageIndex);
		public static string PageSize { get; } = nameof(PageSize);
		public static string SortBy { get; } = nameof(SortBy);
		public static string Ascending { get; } = nameof(Ascending);
	}
}
