using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Razor.SurvivalEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.SurvivalEditor.Store.Features.SpawnsetBinaryFeature;

public class SpawnsetBinaryFeature : Feature<SpawnsetBinaryState>
{
	public override string GetName() => "Spawnset binary";

	protected override SpawnsetBinaryState GetInitialState()
	{
		return new(
			SpawnsetBinary.CreateDefault(),
			"(new spawnset)"
		);
	}
}
