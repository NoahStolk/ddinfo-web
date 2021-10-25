using Fluxor;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Store.Leaderboard.Index;

public class RankFeature : Feature<RankState>
{
	public override string GetName() => "Rank";

	protected override RankState GetInitialState() => new(1);
}
