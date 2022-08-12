using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature;

public class LeaderboardFeature : Feature<LeaderboardState>
{
	public override string GetName() => "Leaderboard";

	protected override LeaderboardState GetInitialState() => new(null, null);
}
