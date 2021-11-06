using DevilDaggersInfo.Web.BlazorWasm.Client.Authentication;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Admin;

public abstract class BaseAdminPage : ComponentBase
{
	[Inject]
	public AdminAuthenticationStateProvider Auth { get; set; } = null!;

	[Inject]
	public NavigationManager NavigationManager { get; set; } = null!;

	[Inject]
	public AdminApiHttpClient Http { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		AuthenticationState state = await Auth.GetAuthenticationStateAsync();
		if (state.User?.Identity?.IsAuthenticated != true)
			NavigationManager.NavigateTo("/authentication/login");
	}
}
