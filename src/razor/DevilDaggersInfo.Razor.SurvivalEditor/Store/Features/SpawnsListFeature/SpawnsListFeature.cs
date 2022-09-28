using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Razor.SurvivalEditor.Store.State;
using DevilDaggersInfo.Types.Core.Spawnsets;
using DevilDaggersInfo.Types.Core.Wiki;
using Fluxor;
using System.Collections.Immutable;

namespace DevilDaggersInfo.Razor.SurvivalEditor.Store.Features.SpawnsListFeature;

public class SpawnsListFeature : Feature<SpawnsListState>
{
	public override string GetName() => "Spawns list";

	protected override SpawnsListState GetInitialState()
	{
		return new(new(GameVersion.V3_2, 6, GameMode.Survival, ImmutableArray<Spawn>.Empty, HandLevel.Level1, 0, 0, 40));
	}
}
