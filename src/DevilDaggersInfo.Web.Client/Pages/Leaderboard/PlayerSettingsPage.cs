using DevilDaggersInfo.Web.ApiSpec.Main.Leaderboards;
using DevilDaggersInfo.Web.ApiSpec.Main.Players;

namespace DevilDaggersInfo.Web.Client.Pages.Leaderboard;

public sealed partial class PlayerSettingsPage
{
	private string? _apiError;

	public List<GetEntry>? GetEntries { get; set; }

	public List<GetPlayerForSettings>? Players { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Players = await Http.GetPlayersForSettings();
		try
		{
			GetEntries = (await Http.GetEntriesByIds(string.Join(',', Players.Select(p => p.Id)))).OrderBy(e => e.Rank).ToList();
		}
		catch (HttpRequestException ex)
		{
			_apiError = ex.Message;
		}
	}
}
