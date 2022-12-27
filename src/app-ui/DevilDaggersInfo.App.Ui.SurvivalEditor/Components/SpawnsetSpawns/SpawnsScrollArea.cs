using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Spawns;
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
		StateManager.Subscribe<LoadSpawnset>(SetSpawns);
		StateManager.Subscribe<SetSpawnsetHistoryIndex>(SetSpawns);
		StateManager.Subscribe<UpdateSpawns>(SetSpawns);
		StateManager.Subscribe<UpdateSpawnsetSetting>(SetSpawns); // Update the spawns when TimerStart or AdditionalGems is changed.
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		bool ctrl = Input.IsCtrlHeld();
		bool shift = Input.IsShiftHeld();

		if (ctrl && Input.IsKeyPressed(Keys.A))
		{
			List<int> newSelectedIndices = _spawnComponents.ConvertAll(sp => sp.Index);
			StateManager.Dispatch(new SetSpawnSelections(newSelectedIndices));
		}

		if (ctrl && Input.IsKeyPressed(Keys.D))
			StateManager.Dispatch(new SetSpawnSelections(new()));

		if (Input.IsKeyPressed(Keys.Delete))
		{
			StateManager.Dispatch(new UpdateSpawns(StateManager.SpawnsetState.Spawnset.Spawns.Where((_, i) => !StateManager.SpawnEditorState.SelectedIndices.Contains(i)).ToImmutableArray(), SpawnsetEditType.SpawnDelete));
			StateManager.Dispatch(new SetSpawnSelections(new()));

			RecalculateHeight();
		}

		bool hoverWithoutBlock = Bounds.Contains(MouseUiContext.MousePosition.RoundToVector2Int32() - scrollOffset);
		if (!Input.IsButtonPressed(MouseButton.Left) || !hoverWithoutBlock)
			return;

		if (shift)
		{
			List<int> newSelectedIndices = StateManager.SpawnEditorState.SelectedIndices.ToList();

			int endIndex = _spawnComponents.Find(sc => sc.Hover)?.Index ?? _spawnComponents.Count - 1;
			int start = Math.Clamp(Math.Min(_currentIndex, endIndex), 0, _spawnComponents.Count - 1);
			int end = Math.Clamp(Math.Max(_currentIndex, endIndex), 0, _spawnComponents.Count - 1);
			for (int i = start; i <= end; i++)
				newSelectedIndices.Add(i);

			StateManager.Dispatch(new SetSpawnSelections(newSelectedIndices));
		}
		else
		{
			List<int> newSelectedIndices = StateManager.SpawnEditorState.SelectedIndices.ToList();

			foreach (SpawnEntry spawnEntry in _spawnComponents)
			{
				if (spawnEntry.Hover)
				{
					if (newSelectedIndices.Contains(spawnEntry.Index))
						newSelectedIndices.Remove(spawnEntry.Index);
					else
						newSelectedIndices.Add(spawnEntry.Index);

					_currentIndex = spawnEntry.Index;
				}
				else if (!ctrl && newSelectedIndices.Contains(spawnEntry.Index))
				{
					newSelectedIndices.Remove(spawnEntry.Index);
				}
			}

			StateManager.Dispatch(new SetSpawnSelections(newSelectedIndices));
		}
	}

	private void SetSpawns()
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
