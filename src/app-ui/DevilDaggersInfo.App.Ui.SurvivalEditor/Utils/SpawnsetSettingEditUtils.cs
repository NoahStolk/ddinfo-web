using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class SpawnsetSettingEditUtils
{
	public static void ChangeSetting<T>(Func<T, SpawnsetBinary> spawnsetBuilder, string input)
		where T : IParsable<T>
	{
		ParseUtils.TryParseAndExecute<T>(input, f => StateManager.SetSpawnset(spawnsetBuilder(f)));
	}
}
