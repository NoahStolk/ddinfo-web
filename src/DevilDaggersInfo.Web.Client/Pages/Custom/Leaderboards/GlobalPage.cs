using DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;
using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;
using DevilDaggersInfo.Web.Client.Extensions;
using DevilDaggersInfo.Web.Client.HttpClients;
using DevilDaggersInfo.Web.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Pages.Custom.Leaderboards;

public partial class GlobalPage
{
	private readonly IReadOnlyList<GameMode> _gameModes = Enum.GetValues<GameMode>();
	private readonly IReadOnlyList<CustomLeaderboardRankSorting> _rankSortings = Enum.GetValues<CustomLeaderboardRankSorting>();

	[Parameter]
	[SupplyParameterFromQuery]
	public string GameMode { get; set; } = nameof(ApiSpec.Main.Spawnsets.GameMode.Survival);

	[Parameter]
	[SupplyParameterFromQuery]
	public string RankSorting { get; set; } = nameof(CustomLeaderboardRankSorting.TimeDesc);

	[Inject]
	public required MainApiHttpClient Http { get; set; }

	[Inject]
	public required NavigationManager NavigationManager { get; set; }

	public GetGlobalCustomLeaderboard? GetGlobalCustomLeaderboard { get; set; }

	private void SetGameMode(GameMode gameMode)
	{
		GameMode = gameMode.ToString();
		NavigationManager.AddOrModifyQueryParameter(nameof(GameMode), GameMode);
	}

	private void SetRankSorting(CustomLeaderboardRankSorting rankSorting)
	{
		RankSorting = rankSorting.ToString();
		NavigationManager.AddOrModifyQueryParameter(nameof(RankSorting), RankSorting);
	}

	protected override async Task OnParametersSetAsync()
	{
		await Fetch();
	}

	private async Task Fetch()
	{
		GameMode gameMode = EnumConvert.GetGameMode(GameMode);
		CustomLeaderboardRankSorting rankSorting = EnumConvert.GetRankSorting(RankSorting);
		GetGlobalCustomLeaderboard = await Http.GetGlobalCustomLeaderboardForCategory(gameMode, rankSorting);
	}
}
