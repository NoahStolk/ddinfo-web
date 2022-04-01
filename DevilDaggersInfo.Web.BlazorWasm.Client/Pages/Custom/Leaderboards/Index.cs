using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Leaderboards;

public partial class Index
{
	[Parameter]
	[SupplyParameterFromQuery]
	public string Category { get; set; } = "Survival";

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

	private Dictionary<CustomLeaderboardSorting, bool> _sortings = new();

	public Page<GetCustomLeaderboardOverview>? GetCustomLeaderboards { get; set; }

	public GetTotalCustomLeaderboardData? GetTotalCustomLeaderboardData { get; set; }

	public int TotalPages => GetCustomLeaderboards == null ? 0 : (GetCustomLeaderboards.TotalResults - 1) / GetPageSize(PageSize) + 1;
	public int TotalResults => GetCustomLeaderboards == null ? 0 : GetCustomLeaderboards.TotalResults;

	protected override async Task OnInitializedAsync()
	{
		foreach (CustomLeaderboardSorting e in (CustomLeaderboardSorting[])Enum.GetValues(typeof(CustomLeaderboardSorting)))
			_sortings.Add(e, false);

		GetTotalCustomLeaderboardData = await Http.GetTotalCustomLeaderboardData();
	}

	protected async override Task OnParametersSetAsync()
	{
		await Fetch();
	}

	private async Task ChangeInputSpawnsetName(ChangeEventArgs e)
	{
		SpawnsetFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(nameof(SpawnsetFilter), SpawnsetFilter);

		await Fetch();
	}

	private async Task ChangeInputAuthorName(ChangeEventArgs e)
	{
		AuthorFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(nameof(AuthorFilter), AuthorFilter);

		await Fetch();
	}

	public async Task ChangePageIndex(int pageIndex)
	{
		PageIndex = Math.Clamp(pageIndex, 0, TotalPages - 1);
		NavigationManager.AddOrModifyQueryParameter(nameof(PageIndex), PageIndex);

		await Fetch();

		// TODO: ??
		StateHasChanged();
	}

	public async Task ChangePageSize(ChangeEventArgs e)
	{
		if (!int.TryParse(e.Value?.ToString(), out int pageSize))
			return;

		PageSize = pageSize;
		NavigationManager.AddOrModifyQueryParameter(nameof(PageSize), PageSize);

		PageIndex = Math.Clamp(PageIndex, 0, TotalPages - 1);
		await Fetch();
	}

	private async Task SetCategory(CustomLeaderboardCategory category)
	{
		Category = EnumConvert.GetString(category);
		NavigationManager.AddOrModifyQueryParameter(nameof(Category), Category);

		await Fetch();
	}

	private async Task Sort(CustomLeaderboardSorting sortBy)
	{
		SortBy = (int)sortBy;
		_sortings[sortBy] = !_sortings[sortBy];
		Ascending = _sortings[sortBy];

		NavigationManager.AddOrModifyQueryParameters(new(nameof(SortBy), SortBy), new(nameof(Ascending), Ascending));

		await Fetch();
	}

	private async Task Fetch()
	{
		CustomLeaderboardCategory category = EnumConvert.GetCustomLeaderboardCategory(Category);
		int pageIndex = Math.Max(0, PageIndex);
		int pageSize = PagingUtils.GetValidPageSize(PageSize);
		CustomLeaderboardSorting sortBy = SortBy.HasValue ? (CustomLeaderboardSorting)SortBy.Value : CustomLeaderboardSorting.DateLastPlayed;

		GetCustomLeaderboards = await Http.GetCustomLeaderboards(category, SpawnsetFilter, AuthorFilter, pageIndex, pageSize, sortBy, Ascending);

		if (PageIndex >= TotalPages)
		{
			PageIndex = TotalPages - 1;
			NavigationManager.AddOrModifyQueryParameter(nameof(PageIndex), PageIndex);
		}
	}
}
