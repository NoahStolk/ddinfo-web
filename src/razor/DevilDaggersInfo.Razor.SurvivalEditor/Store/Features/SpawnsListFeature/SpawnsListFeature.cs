using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Razor.SurvivalEditor.Store.State;
using DevilDaggersInfo.Types.Core.Spawnsets;
using DevilDaggersInfo.Types.Core.Wiki;
using Fluxor;

namespace DevilDaggersInfo.Razor.SurvivalEditor.Store.Features.SpawnsListFeature;

public class SpawnsListFeature : Feature<SpawnsListState>
{
	public override string GetName() => "Spawns list";

	protected override SpawnsListState GetInitialState()
	{
		return new(new(GameMode.Survival, GameVersion.V3_2, Array.Empty<Spawn>()));
	}
}
