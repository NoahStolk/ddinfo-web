using DevilDaggersInfo.Razor.ReplayEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.LeaderboardBrowserFeature;

public class LeaderboardBrowserFeature : Feature<LeaderboardBrowserState>
{
	public override string GetName() => "Leaderboard browser";

	protected override LeaderboardBrowserState GetInitialState() => new(false, 0);
}
