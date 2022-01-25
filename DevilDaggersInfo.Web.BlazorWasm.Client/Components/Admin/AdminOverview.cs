using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;

public partial class AdminOverview<TGetDto>
	where TGetDto : IAdminOverviewGetDto
{
	private string? _sortBy;
	private bool _ascending = true;

	private Page<TGetDto>? _page;
	private string? _errorMessage;
	private bool _errorThrown;
	private Dictionary<PropertyInfo, bool> _properties = ReflectionUtils.GetDtoDisplayProperties<TGetDto>();
	private Dictionary<string, bool> _sortings = null!;

	private Deletion? _deletion;

	private string? _username;

	[Parameter, EditorRequired] public string Title { get; set; } = null!;
	[Parameter, EditorRequired] public Func<int, int, string?, bool, Task<Page<TGetDto>>> ApiCall { get; set; } = null!;
	[Parameter, EditorRequired] public Func<int, Task<HttpResponseMessage>> DeletionApiCall { get; set; } = null!;
	[Parameter, EditorRequired] public string GridConfiguration { get; set; } = null!;

	public int TotalPages => _page == null ? 0 : (_page.TotalResults - 1) / PageSize + 1;
	public int TotalResults => _page == null ? 0 : _page.TotalResults;

	public int PageIndex { get; set; }
	public int PageSize { get; set; } = PagingConstants.PageSizeDefault;

	protected override async Task OnInitializedAsync()
	{
		AuthenticationState auth = await Auth.GetAuthenticationStateAsync();
		_username = auth.User?.GetName();

		_sortings = _properties.ToDictionary(pi => pi.Key.Name, _ => true);
		await Fetch();
	}

	public async Task ChangePageIndex(int pageIndex)
	{
		PageIndex = Math.Clamp(pageIndex, 0, TotalPages - 1);
		await Fetch();
	}

	public async Task ChangePageSize(ChangeEventArgs e)
	{
		_ = int.TryParse(e.Value?.ToString(), out int pageSize);
		PageSize = pageSize;
		PageIndex = Math.Clamp(PageIndex, 0, TotalPages - 1);
		await Fetch();
	}

	private async Task Sort(string propertyName)
	{
		_sortBy = propertyName;
		_sortings[propertyName] = !_sortings[propertyName];
		_ascending = _sortings[propertyName];
		await Fetch();
	}

	private async Task Fetch()
	{
		try
		{
			_page = await ApiCall.Invoke(PageIndex, PageSize, _sortBy, _ascending);
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
