using DevilDaggersInfo.Api.Main.Leaderboards;
using DevilDaggersInfo.Api.Main.Players;

namespace DevilDaggersInfo.Web.Client.Pages.Leaderboard;

public partial class PlayerSettingsPage
{
	public List<GetEntry>? GetEntries { get; set; }

	public List<GetPlayerForSettings>? Players { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Players = await Http.GetPlayersForSettings();
		GetEntries = (await Http.GetEntriesByIds(string.Join(',', Players.Select(p => p.Id)))).OrderBy(e => e.Rank).ToList();
	}
}
