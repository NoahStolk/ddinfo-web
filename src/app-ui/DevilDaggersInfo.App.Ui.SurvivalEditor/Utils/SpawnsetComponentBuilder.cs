using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class SpawnsetComponentBuilder
{
	public static SpawnsetTextInput CreateSpawnsetTextInput(IBounds bounds, Action<string> onChange)
	{
		return new(bounds, true, onChange, onChange, null, GlobalStyles.SpawnsetTextInput);
	}
}
