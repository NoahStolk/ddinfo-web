using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;

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

		return new(rectangle, true, OnInputAndSave, OnInputAndSave, onChange, Color.Black, Color.Gray(0.75f), Color.Gray(0.25f), Color.White, Color.White, Color.Green, Color.Gray(0.5f), 2, FontSize.F8X8, 8, 8);
	}
}
