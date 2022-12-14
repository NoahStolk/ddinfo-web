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
	public required MainApiHttpClient Http { get; set; }

	[Inject]
	public required NavigationManager NavigationManager { get; set; }

	public GetGlobalCustomLeaderboard? GetGlobalCustomLeaderboard { get; set; }

	protected override async Task OnParametersSetAsync()
	{
		await Fetch(Category);
	}

	private async Task Fetch(string category)
	{
		NavigationManager.AddOrModifyQueryParameter(nameof(Category), category);
		GetGlobalCustomLeaderboard = await Http.GetGlobalCustomLeaderboardForCategory(EnumConvert.GetCustomLeaderboardCategory(category));
	}
}
