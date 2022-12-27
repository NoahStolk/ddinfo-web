using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
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

		StateManager.Subscribe<UpdateHandLevel>(SetSpawns);
		StateManager.Subscribe<UpdateAdditionalGems>(SetSpawns);
		StateManager.Subscribe<UpdateTimerStart>(SetSpawns);

		StateManager.Subscribe<AddSpawn>(OnAddSpawn);
		StateManager.Subscribe<EditSpawns>(OnEditSpawns);
		StateManager.Subscribe<InsertSpawn>(OnInsertSpawn);
		StateManager.Subscribe<DeleteSpawns>(SetSpawns);
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		bool ctrl = Input.IsCtrlHeld();
		bool shift = Input.IsShiftHeld();

		// TODO: Only do this when the component is focused.
		if (ctrl && Input.IsKeyPressed(Keys.A))
		{
			List<int> newSelectedIndices = _spawnComponents.ConvertAll(sp => sp.Index);
			StateManager.Dispatch(new SetSpawnSelections(newSelectedIndices));
		}

		// TODO: Only do this when the component is focused.
		if (ctrl && Input.IsKeyPressed(Keys.D))
			StateManager.Dispatch(new SetSpawnSelections(new()));

		// TODO: Only do this when the component is focused.
		if (Input.IsKeyPressed(Keys.Delete))
		{
			StateManager.Dispatch(new DeleteSpawns(StateManager.SpawnsetState.Spawnset.Spawns.Where((_, i) => !StateManager.SpawnEditorState.SelectedIndices.Contains(i)).ToImmutableArray()));
			StateManager.Dispatch(new SetSpawnSelections(new()));

			RecalculateHeight();
		}

		// TODO: Fix this. Right now you can click on File > Save and it will deselect the selected spawns.
		bool hoverWithoutBlock = ContentBounds.Contains(MouseUiContext.MousePosition.RoundToVector2Int32() - scrollOffset);
		if (!Input.IsButtonPressed(MouseButton.Left) || !hoverWithoutBlock)
			return;

		if (shift)
		{
			HashSet<int> newSelectedIndices = StateManager.SpawnEditorState.SelectedIndices.ToHashSet();

			int endIndex = _spawnComponents.Find(sc => sc.Hover)?.Index ?? _spawnComponents.Count - 1;
			int start = Math.Clamp(Math.Min(_currentIndex, endIndex), 0, _spawnComponents.Count - 1);
			int end = Math.Clamp(Math.Max(_currentIndex, endIndex), 0, _spawnComponents.Count - 1);
			for (int i = start; i <= end; i++)
				newSelectedIndices.Add(i);

			StateManager.Dispatch(new SetSpawnSelections(newSelectedIndices.ToList()));
		}
		else if (ctrl)
		{
			HashSet<int> newSelectedIndices = StateManager.SpawnEditorState.SelectedIndices.ToHashSet();

			SpawnEntry? selectedSpawn = _spawnComponents.Find(sc => sc.Hover);

			if (selectedSpawn != null)
			{
				if (newSelectedIndices.Contains(selectedSpawn.Index))
					newSelectedIndices.Remove(selectedSpawn.Index);
				else
					newSelectedIndices.Add(selectedSpawn.Index);

				_currentIndex = selectedSpawn.Index;
			}

			StateManager.Dispatch(new SetSpawnSelections(newSelectedIndices.ToList()));
		}
		else
		{
			HashSet<int> oldSelectedIndices = StateManager.SpawnEditorState.SelectedIndices.ToHashSet();
			HashSet<int> newSelectedIndices = new();

			SpawnEntry? selectedSpawn = _spawnComponents.Find(sc => sc.Hover);

			if (selectedSpawn != null)
			{
				if (!oldSelectedIndices.Contains(selectedSpawn.Index))
					newSelectedIndices.Add(selectedSpawn.Index);

				_currentIndex = selectedSpawn.Index;
			}

			StateManager.Dispatch(new SetSpawnSelections(newSelectedIndices.ToList()));
		}
	}

	private void OnAddSpawn()
	{
		SetSpawns();

		// TODO: We need to call this AFTER the NestingContext has been updated. Even if we call RecalculateHeight here, it still won't be updated until the NestingContext has performed its Update method.
		UpdateScrollOffsetAndScrollbarPosition(new(0, -_spawnComponents.Count * SpawnEntryHeight));
	}

	private void OnEditSpawns()
	{
		SetSpawns();

		if (StateManager.SpawnEditorState.SelectedIndices.Count == 0)
			throw new InvalidOperationException("No spawn selected.");

		// TODO: Only scroll when the spawn is not visible.
		UpdateScrollOffsetAndScrollbarPosition(new(0, -StateManager.SpawnEditorState.SelectedIndices.Min() * SpawnEntryHeight));
	}

	private void OnInsertSpawn()
	{
		// TODO: Scroll to inserted spawn index.
		SetSpawns();
	}

	private void SetSpawns()
	{
		foreach (SpawnEntry component in _spawnComponents)
			NestingContext.Remove(component);

		_spawnComponents.Clear();

		int i = 0;
		foreach (SpawnUiEntry spawn in EditSpawnContext.GetFrom(StateManager.SpawnsetState.Spawnset))
		{
			SpawnEntry spawnEntry = new(Bounds.CreateNested(0, i++ * SpawnEntryHeight, ContentBounds.Size.X, SpawnEntryHeight), spawn);
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
