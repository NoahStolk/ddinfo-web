using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.Client.Pages;
using DevilDaggersInfo.Web.Core.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Components.Admin;

public partial class AdminOverview<TGetDto, TSorting> : IHasNavigation
	where TGetDto : IAdminOverviewGetDto
	where TSorting : struct, Enum
{
	private Page<TGetDto>? _page;
	private string? _errorMessage;
	private bool _errorThrown;
	private Dictionary<TSorting, bool> _sortings = new();

	private Deletion? _deletion;

	private string? _username;

	[Parameter]
	[EditorRequired]
	public required string Title { get; set; }

	[Parameter]
	[EditorRequired]
	public required Func<string?, int, int, TSorting?, bool, Task<Page<TGetDto>>> ApiCall { get; set; }

	[Parameter]
	[EditorRequired]
	public required Func<int, Task<HttpResponseMessage>> DeletionApiCall { get; set; }

	[Parameter]
	[EditorRequired]
	public required List<AdminOverviewColumn<TGetDto, TSorting>> Columns { get; set; } = [];

	[Parameter]
	public string? Filter { get; set; }

	[Parameter]
	public int PageIndex { get; set; }

	[Parameter]
	public int PageSize { get; set; } = Constants.PageSizeDefault;

	[Parameter]
	public int? SortBy { get; set; }

	[Parameter]
	public bool Ascending { get; set; }

	public int TotalPages => _page == null ? 0 : (_page.TotalResults - 1) / PageSize + 1;
	public int TotalResults => _page?.TotalResults ?? 0;

	protected override async Task OnInitializedAsync()
	{
		AuthenticationState auth = await Auth.GetAuthenticationStateAsync();
		_username = auth.User.GetName();

		_sortings = Enum.GetValues<TSorting>().ToDictionary(e => e, _ => true);
	}

	protected override async Task OnParametersSetAsync()
	{
		await FetchAsync();
	}

	private async Task ChangeFilterAsync(ChangeEventArgs e)
	{
		Filter = e.Value?.ToString();

		await FetchAsync();
	}

	public async Task ChangePageIndexAsync(int pageIndex)
	{
		PageIndex = Math.Clamp(pageIndex, 0, TotalPages - 1);

		await FetchAsync();
	}

	public async Task ChangePageSizeAsync(int pageSize)
	{
		PageSize = pageSize;
		PageIndex = Math.Clamp(PageIndex, 0, TotalPages - 1);

		await FetchAsync();
	}

	private async Task SortAsync(TSorting sorting)
	{
		SortBy = (int)(object)sorting;
		_sortings[sorting] = !_sortings[sorting];
		Ascending = _sortings[sorting];

		await FetchAsync();
	}

	private async Task FetchAsync()
	{
		try
		{
			_page = await ApiCall.Invoke(Filter, PageIndex, PageSize, SortBy.HasValue ? (TSorting)(object)SortBy : null, Ascending);
			_errorMessage = null;
			StateHasChanged();
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
		catch (HttpRequestException ex)
		{
			_errorMessage = ex.StatusCode.HasValue ? $"HTTP {(int)ex.StatusCode}: {ex.StatusCode}" : $"An error occurred while sending the request. {ex.Message}";
			_errorThrown = true;
		}
	}

	private void Dismiss()
	{
		_errorMessage = null;
		StateHasChanged();
	}
}
