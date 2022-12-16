using DevilDaggersInfo.Web.Client.Authentication;
using DevilDaggersInfo.Web.Client.HttpClients;
using DevilDaggersInfo.Web.Core.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DevilDaggersInfo.Web.Client.Pages.Admin;

public abstract class BaseAdminPage : ComponentBase
{
	public List<string>? UserRoles { get; private set; }

	[Inject]
	public required AdminAuthenticationStateProvider Auth { get; set; }

	[Inject]
	public required NavigationManager NavigationManager { get; set; }

	[Inject]
	public required AdminApiHttpClient Http { get; set; }

	protected override async Task OnInitializedAsync()
	{
		AuthenticationState state = await Auth.GetAuthenticationStateAsync();
		if (state.User.Identity?.IsAuthenticated != true)
			NavigationManager.NavigateTo("/authentication/login");

		UserRoles = state.User.GetRoles();
	}
}
