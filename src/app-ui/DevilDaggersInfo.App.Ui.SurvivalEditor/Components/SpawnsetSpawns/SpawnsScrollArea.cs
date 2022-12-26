using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Spawns;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.GLFW;
using System.Collections.Immutable;
using Warp.NET;
using Warp.NET.Extensions;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class SpawnsScrollArea : ScrollArea
{
	public const int SpawnEntryHeight = 16;

	private readonly List<SpawnEntry> _spawnComponents = new();

	private int _currentIndex;

	public SpawnsScrollArea(IBounds bounds)
		: base(bounds, 96, 16, GlobalStyles.DefaultScrollAreaStyle)
	{
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

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

			RecalculateHeight();
		}

		bool hoverWithoutBlock = Bounds.Contains(MouseUiContext.MousePosition.RoundToVector2Int32() - scrollOffset);
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
			SpawnEntry spawnEntry = new(Bounds.CreateNested(0, i++ * SpawnEntryHeight, 384, SpawnEntryHeight), spawn);
			_spawnComponents.Add(spawnEntry);
		}

		foreach (SpawnEntry component in _spawnComponents)
			NestingContext.Add(component);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center, Depth, Color.Black);
	}
}
