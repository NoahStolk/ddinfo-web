using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Spawns;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class Spawns : ScrollContent<Spawns, SpawnsWrapper>
{
	public const int SpawnEntryHeight = 24;

	private readonly List<AbstractComponent> _spawnComponents = new();

	public Spawns(Rectangle metric, SpawnsWrapper spawnsWrapper)
		: base(metric, spawnsWrapper)
	{
	}

	public override int ContentHeightInPixels => _spawnComponents.Count * SpawnEntryHeight;

	public void SetSpawnset()
	{
		foreach (AbstractComponent component in _spawnComponents)
			NestingContext.Remove(component);

		_spawnComponents.Clear();

		NestingContext.ScrollOffset = default;

		int i = 0;
		foreach (EditableSpawn spawn in EditSpawnContext.GetFrom(StateManager.SpawnsetState.Spawnset))
		{
			SpawnEntry spawnEntry = new(Rectangle.At(0, i++ * SpawnEntryHeight, 512, SpawnEntryHeight), spawn);
			_spawnComponents.Add(spawnEntry);
		}

		foreach (AbstractComponent component in _spawnComponents)
			NestingContext.Add(component);
	}
}
