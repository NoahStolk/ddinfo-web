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
	public AdminAuthenticationStateProvider Auth { get; set; } = null!;

	[Inject]
	public NavigationManager NavigationManager { get; set; } = null!;

	[Inject]
	public AdminApiHttpClient Http { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		AuthenticationState state = await Auth.GetAuthenticationStateAsync();
		if (state.User?.Identity?.IsAuthenticated != true)
			NavigationManager.NavigateTo("/authentication/login");

		UserRoles = state.User?.GetRoles() ?? new();
	}
}
