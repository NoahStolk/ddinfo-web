using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class SpawnsetSettingEditUtils
{
	public static void ChangeSetting<T>(Func<T, SpawnsetBinary> spawnsetBuilder, string input, string change)
		where T : IParsable<T>
	{
		if (!T.TryParse(input, null, out T v))
			return;

		StateManager.SetSpawnset(spawnsetBuilder(v));
		SpawnsetHistoryManager.Save(change);
	}
}
