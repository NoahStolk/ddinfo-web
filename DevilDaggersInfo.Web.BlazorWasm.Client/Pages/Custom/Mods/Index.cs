using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Mods;

public partial class Index
{
	private int _pageIndex;
	private int _pageSize = PagingConstants.PageSizeDefault;

	[Parameter, SupplyParameterFromQuery] public bool HostedOnly { get; set; }
	[Parameter, SupplyParameterFromQuery] public string? ModFilter { get; set; }
	[Parameter, SupplyParameterFromQuery] public string? AuthorFilter { get; set; }
	[Parameter, SupplyParameterFromQuery] public int PageIndex { get => _pageIndex; set => _pageIndex = Math.Max(0, value); }
	[Parameter, SupplyParameterFromQuery] public int PageSize { get => _pageSize; set => _pageSize = value < PagingConstants.PageSizeMin || value > PagingConstants.PageSizeMax ? PagingConstants.PageSizeDefault : value; }
	[Parameter, SupplyParameterFromQuery] public int? SortBy { get; set; }
	[Parameter, SupplyParameterFromQuery] public bool Ascending { get; set; }

	private Dictionary<ModSorting, bool> _sortings = new();

	public Page<GetModOverview>? GetMods { get; set; }

	public int TotalPages => GetMods == null ? 0 : (GetMods.TotalResults - 1) / PageSize + 1;
	public int TotalResults => GetMods == null ? 0 : GetMods.TotalResults;

	private async Task ChangeInputModName(ChangeEventArgs e)
	{
		ModFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.ModFilter, ModFilter);

		await Fetch();
	}

	private async Task ChangeInputAuthorName(ChangeEventArgs e)
	{
		AuthorFilter = e.Value?.ToString();
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.AuthorFilter, AuthorFilter);

		await Fetch();
	}

	private async Task ChangeInputHostedOnly(ChangeEventArgs e)
	{
		HostedOnly = bool.TryParse(e.Value?.ToString(), out bool value) && value;
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.HostedOnly, HostedOnly);

		await Fetch();
	}

	protected override async Task OnInitializedAsync()
	{
		foreach (ModSorting e in (ModSorting[])Enum.GetValues(typeof(ModSorting)))
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

	private async Task Sort(ModSorting sortBy)
	{
		SortBy = (int)sortBy;
		_sortings[sortBy] = !_sortings[sortBy];
		Ascending = _sortings[sortBy];

		NavigationManager.AddOrModifyQueryParameters(new(QueryParameters.SortBy, SortBy), new(QueryParameters.Ascending, Ascending));

		await Fetch();
	}

	private async Task Fetch()
	{
		GetMods = await Http.GetMods(HostedOnly, ModFilter, AuthorFilter, PageIndex, PageSize, SortBy.HasValue ? (ModSorting)SortBy.Value : ModSorting.LastUpdated, Ascending);

		if (PageIndex >= TotalPages)
		{
			PageIndex = TotalPages - 1;
			NavigationManager.AddOrModifyQueryParameter(QueryParameters.PageIndex, PageIndex);
		}
	}

	private static class QueryParameters
	{
		public static string HostedOnly { get; } = nameof(HostedOnly);
		public static string ModFilter { get; } = nameof(ModFilter);
		public static string AuthorFilter { get; } = nameof(AuthorFilter);
		public static string PageIndex { get; } = nameof(PageIndex);
		public static string PageSize { get; } = nameof(PageSize);
		public static string SortBy { get; } = nameof(SortBy);
		public static string Ascending { get; } = nameof(Ascending);
	}
}
