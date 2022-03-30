using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Leaderboards;

public partial class GlobalPage
{
	[Parameter]
	[SupplyParameterFromQuery]
	public string Category { get; set; } = "Survival";

	[Inject]
	public PublicApiHttpClient Http { get; set; } = null!;

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
