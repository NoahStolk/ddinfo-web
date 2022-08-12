using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature;

public class SpawnsetFeature : Feature<SpawnsetState>
{
	public override string GetName() => "Spawnset";

	protected override SpawnsetState GetInitialState() => new(false, null, null);
}
