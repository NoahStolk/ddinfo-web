using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Spawns;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.GLFW;
using System.Collections.Immutable;
using Warp.NET;
using Warp.NET.Extensions;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class Spawns : ScrollContent<Spawns, SpawnsWrapper>
{
	public const int SpawnEntryHeight = 16;

	private readonly List<SpawnEntry> _spawnComponents = new();

	private int _currentIndex;

	public Spawns(IBounds bounds, SpawnsWrapper parent)
		: base(bounds, parent)
	{
	}

	public override int ContentHeightInPixels => _spawnComponents.Count * SpawnEntryHeight;

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		bool ctrl = Input.IsCtrlHeld();
		bool shift = Input.IsShiftHeld();

		if (ctrl && Input.IsKeyPressed(Keys.A))
			_spawnComponents.ForEach(sp => StateManager.SelectSpawn(sp.Index));

		if (ctrl && Input.IsKeyPressed(Keys.D))
			StateManager.ClearSpawnSelections();

		if (Input.IsKeyPressed(Keys.Delete))
		{
			StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with
			{
				Spawns = StateManager.SpawnsetState.Spawnset.Spawns.Where((_, i) => !StateManager.SpawnEditorState.SelectedIndices.Contains(i)).ToImmutableArray(),
			});
			SpawnsetHistoryManager.Save(SpawnsetEditType.SpawnDelete);
			StateManager.ClearSpawnSelections();
		}

		bool hoverWithoutBlock = Bounds.Contains(MouseUiContext.MousePosition.RoundToVector2Int32() - parentPosition);
		if (!Input.IsButtonPressed(MouseButton.Left) || !hoverWithoutBlock)
			return;

		if (shift)
		{
			int endIndex = _spawnComponents.Find(sc => sc.Hover)?.Index ?? _spawnComponents.Count - 1;
			int start = Math.Clamp(Math.Min(_currentIndex, endIndex), 0, _spawnComponents.Count - 1);
			int end = Math.Clamp(Math.Max(_currentIndex, endIndex), 0, _spawnComponents.Count - 1);
			for (int i = start; i <= end; i++)
				StateManager.SelectSpawn(i);
		}
		else
		{
			foreach (SpawnEntry spawnEntry in _spawnComponents)
			{
				if (spawnEntry.Hover)
				{
					StateManager.ToggleSpawnSelection(spawnEntry.Index);
					_currentIndex = spawnEntry.Index;
				}
				else if (!ctrl)
				{
					StateManager.DeselectSpawn(spawnEntry.Index);
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
