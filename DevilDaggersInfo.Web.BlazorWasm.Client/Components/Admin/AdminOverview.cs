using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Client.Pages;
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;

public partial class AdminOverview<TGetDto, TSorting> : IHasNavigation
	where TGetDto : IAdminOverviewGetDto
	where TSorting : struct, Enum
{
	private Page<TGetDto>? _page;
	private string? _errorMessage;
	private bool _errorThrown;
	private Dictionary<TSorting, bool> _sortings = null!;

	private Deletion? _deletion;

	private string? _username;

	[Parameter]
	[EditorRequired]
	public string Title { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public Func<int, int, TSorting?, bool, Task<Page<TGetDto>>> ApiCall { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public Func<int, Task<HttpResponseMessage>> DeletionApiCall { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public string GridConfiguration { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public RenderFragment TableHeader { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public RenderFragment<TGetDto> RowTemplate { get; set; } = null!;

	[Parameter]
	public int PageIndex { get; set; }

	[Parameter]
	public int PageSize { get; set; } = PagingConstants.PageSizeDefault;

	[Parameter]
	public int? SortBy { get; set; }

	[Parameter]
	public bool Ascending { get; set; }

	public int TotalPages => _page == null ? 0 : (_page.TotalResults - 1) / PageSize + 1;
	public int TotalResults => _page == null ? 0 : _page.TotalResults;

	protected override async Task OnInitializedAsync()
	{
		AuthenticationState auth = await Auth.GetAuthenticationStateAsync();
		_username = auth.User?.GetName();

		_sortings = Enum.GetValues<TSorting>().ToDictionary(e => e, _ => true);
	}

	protected override async Task OnParametersSetAsync()
	{
		await Fetch();
	}

	public async Task ChangePageIndex(int pageIndex)
	{
		PageIndex = Math.Clamp(pageIndex, 0, TotalPages - 1);

		await Fetch();
	}

	public async Task ChangePageSize(int pageSize)
	{
		PageSize = pageSize;
		PageIndex = Math.Clamp(PageIndex, 0, TotalPages - 1);

		await Fetch();
	}

	public void Sort(TSorting sorting)
	{
		SortBy = (int)(object)sorting;
		_sortings[sorting] = !_sortings[sorting];
		Ascending = _sortings[sorting];
	}

	private async Task Fetch()
	{
		try
		{
			_page = await ApiCall.Invoke(PageIndex, PageSize, SortBy.HasValue ? (TSorting)(object)SortBy : null, Ascending);
			_errorMessage = null;
			StateHasChanged();
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
		catch (HttpRequestException ex)
		{
			if (ex.StatusCode.HasValue)
				_errorMessage = $"HTTP {(int)ex.StatusCode}: {ex.StatusCode}";
			else
				_errorMessage = $"An error occurred while sending the request. {ex.Message}";

			_errorThrown = true;
		}
	}

	private void Dismiss()
	{
		_errorMessage = null;
		StateHasChanged();
	}
}
