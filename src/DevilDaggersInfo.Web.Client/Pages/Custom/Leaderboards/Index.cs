using DevilDaggersInfo.Web.ApiSpec.Main;
using DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;
using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;
using DevilDaggersInfo.Web.Client.Extensions;
using DevilDaggersInfo.Web.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Pages.Custom.Leaderboards;

public partial class Index : IHasNavigation
{
	private readonly IReadOnlyList<GameMode> _gameModes = Enum.GetValues<GameMode>();
	private readonly Dictionary<CustomLeaderboardSorting, bool> _sortings = new();

	private IReadOnlyList<CategoryDropdown>? _categories;

	[Parameter]
	[SupplyParameterFromQuery]
	public string GameMode { get; set; } = nameof(ApiSpec.Main.Spawnsets.GameMode.Survival);

	[Parameter]
	[SupplyParameterFromQuery]
	public string RankSorting { get; set; } = nameof(CustomLeaderboardRankSorting.TimeDesc);

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

	public Page<GetCustomLeaderboardOverview>? GetCustomLeaderboards { get; set; }

	public GetTotalCustomLeaderboardData? GetTotalCustomLeaderboardData { get; set; }

	public int TotalPages => GetCustomLeaderboards == null ? 0 : (GetCustomLeaderboards.TotalResults - 1) / PagingUtils.GetValidPageSize(PageSize) + 1;
	public int TotalResults => GetCustomLeaderboards?.TotalResults ?? 0;

	protected override async Task OnInitializedAsync()
	{
		foreach (CustomLeaderboardSorting e in (CustomLeaderboardSorting[])Enum.GetValues(typeof(CustomLeaderboardSorting)))
			_sortings.Add(e, false);

		GetTotalCustomLeaderboardData = await Http.GetTotalCustomLeaderboardData();

		List<GetCustomLeaderboardAllowedCategory> allowedCategories = await Http.GetCustomLeaderboardAllowedCategories();
		_categories = allowedCategories.ConvertAll(a => new CategoryDropdown(a.GameMode, a.RankSorting));
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

	private void SetCategory(CategoryDropdown categoryDropdown)
	{
		GameMode = categoryDropdown.GameMode.ToString();
		RankSorting = categoryDropdown.RankSorting.ToString();

		NavigationManager.AddOrModifyQueryParameters(new(nameof(GameMode), GameMode), new(nameof(RankSorting), RankSorting));
	}

	private void Sort(CustomLeaderboardSorting sortBy)
	{
		SortBy = (int)sortBy;
		_sortings[sortBy] = !_sortings[sortBy];
		Ascending = _sortings[sortBy];

		NavigationManager.AddOrModifyQueryParameters(new(nameof(SortBy), SortBy), new(nameof(Ascending), Ascending));
	}

	private async Task Fetch()
	{
		GameMode gameMode = EnumConvert.GetGameMode(GameMode);
		CustomLeaderboardRankSorting rankSorting = EnumConvert.GetRankSorting(RankSorting);
		int pageIndex = Math.Max(0, PageIndex);
		int pageSize = PagingUtils.GetValidPageSize(PageSize);
		CustomLeaderboardSorting sortBy = SortBy.HasValue ? (CustomLeaderboardSorting)SortBy.Value : CustomLeaderboardSorting.DateLastPlayed;

		GetCustomLeaderboards = await Http.GetCustomLeaderboards(gameMode, rankSorting, SpawnsetFilter, AuthorFilter, pageIndex, pageSize, sortBy, Ascending);

		if (PageIndex >= TotalPages)
		{
			PageIndex = TotalPages - 1;
			NavigationManager.AddOrModifyQueryParameter(nameof(PageIndex), PageIndex);
		}
	}

	private sealed class CategoryDropdown
	{
		public CategoryDropdown(GameMode gameMode, CustomLeaderboardRankSorting rankSorting)
		{
			GameMode = gameMode;
			RankSorting = rankSorting;
		}

		public GameMode GameMode { get; }
		public CustomLeaderboardRankSorting RankSorting { get; }

		public override string ToString()
		{
			return $"{GameMode.ToCore().ToDisplayString()}: {RankSorting.ToDisplayString()}";
		}
	}
}
