using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Web.Client.Extensions;
using DevilDaggersInfo.Web.Client.HttpClients;
using DevilDaggersInfo.Web.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Pages.Custom.Leaderboards;

public partial class GlobalPage
{
	[Parameter]
	[SupplyParameterFromQuery]
	public string Category { get; set; } = "Survival";

	[Inject]
	public MainApiHttpClient Http { get; set; } = null!;

	[Inject]
	public NavigationManager NavigationManager { get; set; } = null!;

	public GetGlobalCustomLeaderboard? GetGlobalCustomLeaderboard { get; set; }

	protected async override Task OnParametersSetAsync()
	{
		await Fetch(Category);
	}

	private async Task Fetch(string category)
	{
		NavigationManager.AddOrModifyQueryParameter(nameof(Category), category);
		GetGlobalCustomLeaderboard = await Http.GetGlobalCustomLeaderboardForCategory(EnumConvert.GetCustomLeaderboardCategory(category));
	}
}
