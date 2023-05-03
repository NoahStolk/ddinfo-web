using DevilDaggersInfo.App.Ui.SurvivalEditor.EditorSpawns;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class SpawnsScrollArea : ScrollArea
{
	public const int SpawnEntryHeight = 16;

	private readonly List<SpawnEntry> _spawnComponents = new();

	private int _currentIndex;

	public SpawnsScrollArea(IBounds bounds)
		: base(bounds, 96, 16, ScrollAreaStyles.Default)
	{
		StateManager.Subscribe<LoadSpawnset>(SetSpawns);
		StateManager.Subscribe<SetSpawnsetHistoryIndex>(SetSpawns);

		StateManager.Subscribe<UpdateHandLevel>(SetSpawns);
		StateManager.Subscribe<UpdateAdditionalGems>(SetSpawns);
		StateManager.Subscribe<UpdateTimerStart>(SetSpawns);

		StateManager.Subscribe<AddSpawn>(SetSpawns);
		StateManager.Subscribe<EditSpawns>(SetSpawns);
		StateManager.Subscribe<InsertSpawn>(SetSpawns);
		StateManager.Subscribe<DeleteSpawns>(SetSpawns);

		StateManager.Subscribe<AddSpawn>(ScrollToEnd);
		StateManager.Subscribe<EditSpawns>(ScrollToFirstSelectedIndex);
		StateManager.Subscribe<InsertSpawn>(ScrollToInsertedSpawn);
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		// TODO: Only do this when the component is focused.
		if (Input.IsKeyPressed(Keys.Delete))
		{
			StateManager.Dispatch(new DeleteSpawns(StateManager.SpawnsetState.Spawnset.Spawns.Where((_, i) => !StateManager.SpawnEditorState.SelectedIndices.Contains(i)).ToImmutableArray()));
			StateManager.Dispatch(new SetSpawnSelections(new()));
		}

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
	}
}
