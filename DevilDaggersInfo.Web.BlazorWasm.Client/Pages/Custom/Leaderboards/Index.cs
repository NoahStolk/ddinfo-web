using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Leaderboards;

public partial class Index
{
	private int _pageIndex;
	private int _pageSize = PagingConstants.PageSizeDefault;

	[Parameter]
	[SupplyParameterFromQuery]
	public string Category { get; set; } = "Survival";

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
		NavigationManager.AddOrModifyQueryParameter(nameof(SpawnsetFilter), SpawnsetFilter);

		await Fetch(Category);
	}

	private async Task ChangeInputAuthorName(ChangeEventArgs e)
	{
		AuthorFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(nameof(AuthorFilter), AuthorFilter);

		await Fetch(Category);
	}

	protected override async Task OnInitializedAsync()
	{
		foreach (CustomLeaderboardSorting e in (CustomLeaderboardSorting[])Enum.GetValues(typeof(CustomLeaderboardSorting)))
			_sortings.Add(e, false);

		await Fetch(Category);

		GetTotalCustomLeaderboardData = await Http.GetTotalCustomLeaderboardData();
	}

	public async Task ChangePageIndex(int pageIndex)
	{
		PageIndex = Math.Clamp(pageIndex, 0, TotalPages - 1);
		NavigationManager.AddOrModifyQueryParameter(nameof(PageIndex), PageIndex);

		await Fetch(Category);

		StateHasChanged();
	}

	public async Task ChangePageSize(ChangeEventArgs e)
	{
		if (!int.TryParse(e.Value?.ToString(), out int pageSize))
			return;

		PageSize = pageSize;
		NavigationManager.AddOrModifyQueryParameter(nameof(PageSize), PageSize);

		PageIndex = Math.Clamp(PageIndex, 0, TotalPages - 1);
		await Fetch(Category);
	}

	private async Task Sort(CustomLeaderboardSorting sortBy)
	{
		SortBy = (int)sortBy;
		_sortings[sortBy] = !_sortings[sortBy];
		Ascending = _sortings[sortBy];

		NavigationManager.AddOrModifyQueryParameters(new(nameof(SortBy), SortBy), new(nameof(Ascending), Ascending));

		await Fetch(Category);
	}

	protected async override Task OnParametersSetAsync()
	{
		await Fetch(Category);
	}

	private async Task Fetch(string category)
	{
		NavigationManager.AddOrModifyQueryParameter(nameof(Category), category);

		GetCustomLeaderboards = await Http.GetCustomLeaderboards(EnumConvert.GetCustomLeaderboardCategory(category), SpawnsetFilter, AuthorFilter, PageIndex, PageSize, SortBy.HasValue ? (CustomLeaderboardSorting)SortBy.Value : CustomLeaderboardSorting.DateLastPlayed, Ascending);

		if (PageIndex >= TotalPages)
		{
			PageIndex = TotalPages - 1;
			NavigationManager.AddOrModifyQueryParameter(nameof(PageIndex), PageIndex);
		}
	}
}
