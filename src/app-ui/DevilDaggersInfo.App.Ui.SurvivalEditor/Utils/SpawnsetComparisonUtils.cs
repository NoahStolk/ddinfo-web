using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class SpawnsetComparisonUtils
{
	public static bool HasArenaChanges(SpawnsetBinary a, SpawnsetBinary b)
	{
		if (HasArenaSettingsChanges(a, b))
			return true;

		if (a.ArenaDimension != b.ArenaDimension)
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

	public static bool HasSpawnsChanges(SpawnsetBinary a, SpawnsetBinary b)
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

	public static bool HasSettingsChanges(SpawnsetBinary a, SpawnsetBinary b)
	{
		return HasArenaSettingsChanges(a, b)
			|| a.SpawnVersion != b.SpawnVersion
			|| a.WorldVersion != b.WorldVersion
			|| a.Brightness != b.Brightness
			|| a.GameMode != b.GameMode
			|| a.HandLevel != b.HandLevel
			|| a.AdditionalGems != b.AdditionalGems
			|| a.TimerStart != b.TimerStart;
	}

	public static bool HasArenaSettingsChanges(SpawnsetBinary a, SpawnsetBinary b)
	{
		return a.ShrinkStart != b.ShrinkStart
			|| a.ShrinkEnd != b.ShrinkEnd
			|| a.ShrinkRate != b.ShrinkRate;
	}
}
