using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.States;

public static class StateManager
{
	private static UiQueue? _uiQueue;

	public static SpawnsetState SpawnsetState { get; private set; } = SpawnsetState.GetDefault();

	public static ArenaEditorState ArenaEditorState { get; private set; } = ArenaEditorState.GetDefault();

	public static SpawnEditorState SpawnEditorState { get; private set; } = SpawnEditorState.GetDefault();

	public static void EmptyUiQueue()
	{
		if (_uiQueue == null)
			return;

		Root.Game.SurvivalEditorMainLayout.SetSpawnset(_uiQueue.ArenaChanges, _uiQueue.SpawnsChanges, _uiQueue.SettingsChanges);
		_uiQueue = null;
	}

	public static void NewSpawnset()
	{
		SetSpawnset("(untitled)", SpawnsetBinary.CreateDefault());
	}

	public static void OpenDefaultV3Spawnset()
	{
		SetSpawnset("(untitled)", ContentManager.Content.DefaultSpawnset.DeepCopy());
	}

	public static void SetSpawnset(string spawnsetName, SpawnsetBinary spawnsetBinary)
	{
		SpawnsetState = new(spawnsetName, spawnsetBinary);

		Root.Game.SurvivalEditorMainLayout.SetSpawnset(true, true, true);
		SpawnsetHistoryManager.Reset();
	}

	public static void SetSpawnset(SpawnsetBinary spawnsetBinary)
	{
		bool arenaChanges = SpawnsetComparisonUtils.HasArenaChanges(SpawnsetState.Spawnset, spawnsetBinary);
		bool spawnsChanges = SpawnsetComparisonUtils.HasSpawnsChanges(SpawnsetState.Spawnset, spawnsetBinary);
		bool settingsChanges = SpawnsetComparisonUtils.HasSettingsChanges(SpawnsetState.Spawnset, spawnsetBinary);

		SpawnsetState = SpawnsetState with { Spawnset = spawnsetBinary };

		if (_uiQueue == null)
			_uiQueue = new(arenaChanges, spawnsChanges, settingsChanges);
		else
			_uiQueue = new(_uiQueue.ArenaChanges || arenaChanges, _uiQueue.SpawnsChanges || spawnsChanges, _uiQueue.SettingsChanges || settingsChanges);
	}

	public static void ReplaceSpawnset()
	{
		File.WriteAllBytes(Path.Combine(UserSettings.DevilDaggersInstallationDirectory, "mods", "survival"), SpawnsetState.Spawnset.ToBytes());
		ILayout popupParent = Root.Game.SurvivalEditorMainLayout;
		Popup p = new(popupParent, "Successfully replaced current survival file");
		popupParent.NestingContext.Add(p);
	}

	public static void SetArenaTool(ArenaTool arenaTool)
	{
		ArenaEditorState = ArenaEditorState with { ArenaTool = arenaTool };
	}

	public static void SetArenaSelectedHeight(float selectedHeight)
	{
		ArenaEditorState = ArenaEditorState with { SelectedHeight = selectedHeight };
	}

	public static void SetArenaBucketTolerance(float bucketTolerance)
	{
		ArenaEditorState = ArenaEditorState with { BucketTolerance = bucketTolerance };
	}

	public static void SetArenaBucketVoidHeight(float bucketVoidHeight)
	{
		ArenaEditorState = ArenaEditorState with { BucketVoidHeight = bucketVoidHeight };
	}

	// TODO: Mutable list OK?
	public static void SelectSpawn(int index)
	{
		if (!SpawnEditorState.SelectedIndices.Contains(index))
			SpawnEditorState.SelectedIndices.Add(index);
	}

	public static void DeselectSpawn(int index)
	{
		if (SpawnEditorState.SelectedIndices.Contains(index))
			SpawnEditorState.SelectedIndices.Remove(index);
	}

	public static void ToggleSpawnSelection(int index)
	{
		if (SpawnEditorState.SelectedIndices.Contains(index))
			SpawnEditorState.SelectedIndices.Remove(index);
		else
			SpawnEditorState.SelectedIndices.Add(index);
	}

	public static void ClearSpawnSelections()
	{
		SpawnEditorState.SelectedIndices.Clear();
	}

	/// <summary>
	/// The component system can only handle one consecutive update of spawnset components every update iteration, so in case of multiple updates, schedule an update and update the components on the next update.
	/// </summary>
	private sealed record UiQueue(bool ArenaChanges, bool SpawnsChanges, bool SettingsChanges);
}
