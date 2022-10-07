using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Spawns;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.GLFW;
using System.Collections.Immutable;
using Warp;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class Spawns : ScrollContent<Spawns, SpawnsWrapper>
{
	public const int SpawnEntryHeight = 16;

	private readonly List<SpawnEntry> _spawnComponents = new();

	private int _currentIndex;

	public Spawns(Rectangle metric, SpawnsWrapper spawnsWrapper)
		: base(metric, spawnsWrapper)
	{
	}

	public override int ContentHeightInPixels => _spawnComponents.Count * SpawnEntryHeight;

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		bool ctrl = Input.IsKeyHeld(Keys.ControlLeft) || Input.IsKeyHeld(Keys.ControlRight);
		bool shift = Input.IsKeyHeld(Keys.ShiftLeft) || Input.IsKeyHeld(Keys.ShiftRight);

		if (ctrl && Input.IsKeyPressed(Keys.A))
			_spawnComponents.ForEach(sp => sp.IsSelected = true);

		if (Input.IsKeyPressed(Keys.Delete))
		{
			StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with
			{
				Spawns = StateManager.SpawnsetState.Spawnset.Spawns.Where((_, i) => !_spawnComponents[i].IsSelected).ToImmutableArray(),
			});
			SpawnsetHistoryManager.Save(SpawnsetEditType.SpawnDelete);
		}

		if (!Input.IsButtonPressed(MouseButton.Left))
			return;

		if (shift)
		{
			int endIndex = _spawnComponents.Find(sc => sc.Hover)?.Index ?? 0;
			int start = Math.Clamp(Math.Min(_currentIndex, endIndex), 0, _spawnComponents.Count - 1);
			int end = Math.Clamp(Math.Max(_currentIndex, endIndex), 0, _spawnComponents.Count - 1);
			for (int i = start; i <= end; i++)
				_spawnComponents[i].IsSelected = true;
		}
		else
		{
			foreach (SpawnEntry spawnEntry in _spawnComponents)
			{
				if (spawnEntry.Hover)
				{
					spawnEntry.IsSelected = !spawnEntry.IsSelected;
					_currentIndex = spawnEntry.Index;
				}
				else if (!ctrl)
				{
					spawnEntry.IsSelected = false;
				}
			}
		}
	}

	public void SetSpawnset()
	{
		foreach (SpawnEntry component in _spawnComponents)
			NestingContext.Remove(component);

		_spawnComponents.Clear();

		int i = 0;
		foreach (SpawnUiEntry spawn in EditSpawnContext.GetFrom(StateManager.SpawnsetState.Spawnset))
		{
			SpawnEntry spawnEntry = new(Rectangle.At(0, i++ * SpawnEntryHeight, 384, SpawnEntryHeight), spawn);
			_spawnComponents.Add(spawnEntry);
		}

		foreach (SpawnEntry component in _spawnComponents)
			NestingContext.Add(component);
	}
}
