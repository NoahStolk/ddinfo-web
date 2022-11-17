using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class SpawnsetComponentBuilder
{
	public static SpawnsetTextInput CreateSpawnsetTextInput(Rectangle rectangle, Action<string> onChange, SpawnsetEditType spawnsetEditType)
	{
		void OnInputAndSave(string input)
		{
			onChange(input);
			SpawnsetHistoryManager.Save(spawnsetEditType);
		}

		return new(rectangle, true, OnInputAndSave, OnInputAndSave, onChange, GlobalStyles.SpawnsetTextInput);
	}
}
