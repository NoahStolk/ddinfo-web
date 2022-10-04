using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.States;

public static class StateManager
{
	private static UiQueue? _uiQueue;

	public static SpawnsetState SpawnsetState { get; private set; } = SpawnsetState.GetDefault();

	public static ArenaEditorState ArenaEditorState { get; private set; } = ArenaEditorState.GetDefault();

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
		bool arenaChanges = HasArenaChanges(SpawnsetState.Spawnset, spawnsetBinary);
		bool spawnsChanges = HasSpawnsChanges(SpawnsetState.Spawnset, spawnsetBinary);
		bool settingsChanges = HasSettingsChanges(SpawnsetState.Spawnset, spawnsetBinary);

		SpawnsetState = SpawnsetState with { Spawnset = spawnsetBinary };

		if (_uiQueue == null)
			_uiQueue = new(arenaChanges, spawnsChanges, settingsChanges);
		else
			_uiQueue = new(_uiQueue.ArenaChanges || arenaChanges, _uiQueue.SpawnsChanges || spawnsChanges, _uiQueue.SettingsChanges || settingsChanges);
	}

	public static void EmptyUiQueue()
	{
		if (_uiQueue == null)
			return;

		Root.Game.SurvivalEditorMainLayout.SetSpawnset(_uiQueue.ArenaChanges, _uiQueue.SpawnsChanges, _uiQueue.SettingsChanges);
		_uiQueue = null;
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

	private static bool HasArenaChanges(SpawnsetBinary a, SpawnsetBinary b)
	{
		if (a.ShrinkStart != b.ShrinkStart || a.ShrinkEnd != b.ShrinkEnd || a.ShrinkRate != b.ShrinkRate || a.Brightness != b.Brightness || a.ArenaDimension != b.ArenaDimension)
			return true;

		for (int i = 0; i < a.ArenaDimension; i++)
		{
			for (int j = 0; j < a.ArenaDimension; j++)
			{
				if (a.ArenaTiles[i, j] != b.ArenaTiles[i, j])
					return true;
			}
		}

		return false;
	}

	private static bool HasSpawnsChanges(SpawnsetBinary a, SpawnsetBinary b)
	{
		if (a.Spawns.Length != b.Spawns.Length)
			return true;

		for (int i = 0; i < a.Spawns.Length; i++)
		{
			if (a.Spawns[i].EnemyType != b.Spawns[i].EnemyType)
				return true;

			if (a.Spawns[i].Delay != b.Spawns[i].Delay)
				return true;
		}

		return false;
	}

	private static bool HasSettingsChanges(SpawnsetBinary a, SpawnsetBinary b)
	{
		return a.SpawnVersion != b.SpawnVersion
			|| a.WorldVersion != b.WorldVersion
			|| a.GameMode != b.GameMode
			|| a.HandLevel != b.HandLevel
			|| a.AdditionalGems != b.AdditionalGems
			|| a.TimerStart != b.TimerStart;
	}

	/// <summary>
	/// The component system can only handle one consecutive update of spawnset components every update iteration, so in case of multiple updates, schedule an update and update the components on the next update.
	/// </summary>
	private sealed record UiQueue(bool ArenaChanges, bool SpawnsChanges, bool SettingsChanges);
}
