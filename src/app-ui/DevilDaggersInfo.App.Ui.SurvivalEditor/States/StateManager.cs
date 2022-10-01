using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.States;

public static class StateManager
{
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
		bool arenaChanges = ArenaChanges(SpawnsetState.Spawnset, spawnsetBinary);
		bool settingsChanges = SettingsChanges(SpawnsetState.Spawnset, spawnsetBinary);
		bool spawnsChanges = SpawnsChanges(SpawnsetState.Spawnset, spawnsetBinary);

		SpawnsetState = SpawnsetState with { Spawnset = spawnsetBinary };

		Root.Game.SurvivalEditorMainLayout.SetSpawnset(arenaChanges, spawnsChanges, settingsChanges);

		static bool ArenaChanges(SpawnsetBinary a, SpawnsetBinary b)
		{
			if (a.ShrinkStart != b.ShrinkStart || a.ShrinkEnd != b.ShrinkEnd || a.ShrinkRate != b.ShrinkRate || a.ArenaDimension != b.ArenaDimension)
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

		static bool SpawnsChanges(SpawnsetBinary a, SpawnsetBinary b)
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

		static bool SettingsChanges(SpawnsetBinary a, SpawnsetBinary b)
		{
			return a.SpawnVersion != b.SpawnVersion
				|| a.WorldVersion != b.WorldVersion
				|| a.GameMode != b.GameMode
				|| a.HandLevel != b.HandLevel
				|| a.AdditionalGems != b.AdditionalGems
				|| a.TimerStart != b.TimerStart;
		}
	}

	public static void SetArenaTool(ArenaTool arenaTool)
	{
		ArenaEditorState = ArenaEditorState with { ArenaTool = arenaTool };
	}

	public static void SetArenaSelectedHeight(float selectedHeight)
	{
		ArenaEditorState = ArenaEditorState with { SelectedHeight = selectedHeight };
	}
}
