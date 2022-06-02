using DevilDaggersInfo.Api.Admin.Users;
using DevilDaggersInfo.Web.Client.Components.Admin;
using DevilDaggersInfo.Web.Shared;
using DevilDaggersInfo.Web.Shared.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Pages.Admin.Users;

public partial class Index
{
	private List<GetUser>? _users;
	private string? _errorMessage;
	private bool _errorThrown;

	private Deletion? _deletion;

	private bool? IsAdmin => UserRoles == null ? null : UserRoles.Contains(Roles.Admin);

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

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
