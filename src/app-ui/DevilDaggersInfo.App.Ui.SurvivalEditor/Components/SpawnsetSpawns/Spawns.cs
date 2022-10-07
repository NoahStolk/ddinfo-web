using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Spawns;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.GLFW;
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

		if (!Input.IsButtonPressed(MouseButton.Left))
			return;

		bool ctrl = Input.IsKeyHeld(Keys.ControlLeft) || Input.IsKeyHeld(Keys.ControlRight);
		bool shift = Input.IsKeyHeld(Keys.ShiftLeft) || Input.IsKeyHeld(Keys.ShiftRight);

		if (shift)
		{
			int endIndex = _spawnComponents.Find(sc => sc.Hover)?.Index ?? 0;
			for (int i = Math.Min(_currentIndex, endIndex); i <= Math.Max(_currentIndex, endIndex); i++)
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

		NestingContext.ScrollOffset = default;

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
