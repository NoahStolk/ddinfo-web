using DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Users;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Admin.Users;

public partial class Index
{
	private List<GetUser>? _users;
	private string? _errorMessage;
	private bool _errorThrown;

	private Deletion? _deletion;

	private string? _username;

	protected override async Task OnInitializedAsync()
	{
		AuthenticationState auth = await Auth.GetAuthenticationStateAsync();
		_username = auth.User?.GetName();

		await Fetch();
	}

	private async Task Fetch()
	{
		try
		{
			_users = await Http.GetUsers();
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

	private async Task ToggleRole(int userId, string roleName)
	{
		await Http.ToggleRole(userId, new() { RoleName = roleName });
		await Fetch();
	}

	private void Dismiss()
	{
		_errorMessage = null;
		StateHasChanged();
	}
}
