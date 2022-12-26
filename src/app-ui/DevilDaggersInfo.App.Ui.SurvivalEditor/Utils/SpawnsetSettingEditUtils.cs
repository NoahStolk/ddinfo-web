using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class SpawnsetSettingEditUtils
{
	public static void ChangeSetting<T>(Func<T, SpawnsetBinary> spawnsetBuilder, string input)
		where T : IParsable<T>
	{
		ParseUtils.TryParseAndExecute<T>(input, f => StateManager.Dispatch(new UpdateSpawnsetSetting(spawnsetBuilder(f))));
	}
}
