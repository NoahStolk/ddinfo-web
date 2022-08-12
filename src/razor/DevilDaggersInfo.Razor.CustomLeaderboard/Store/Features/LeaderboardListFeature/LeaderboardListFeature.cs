using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardListFeature;

public class LeaderboardListFeature : Feature<LeaderboardListState>
{
	public override string GetName() => "Leaderboard list";

	protected override LeaderboardListState GetInitialState() => new(false, null, null, 0, Types.Web.CustomLeaderboardCategory.Survival, 0, 20);
}
