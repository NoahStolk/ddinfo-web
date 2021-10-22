using Fluxor;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Store.Leaderboard.Index;

public class ChangeRankReducer : Reducer<RankState, ChangeRankAction>
{
	public override RankState Reduce(RankState state, ChangeRankAction action)
	{
		int clampedRank = Math.Clamp(action.Rank, 1, action.MaxRank);
		return new(clampedRank);
	}
}
