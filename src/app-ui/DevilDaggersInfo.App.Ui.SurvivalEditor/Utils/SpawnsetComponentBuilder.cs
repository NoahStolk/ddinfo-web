using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class SpawnsetComponentBuilder
{
	public static SpawnsetTextInput CreateSpawnsetTextInput(IBounds bounds, Action<string> onChange, SpawnsetEditType spawnsetEditType)
	{
		void OnInputAndSave(string input)
		{
			onChange(input);
			StateManager.Dispatch(new SaveHistory(spawnsetEditType));
		}

		return new(bounds, true, OnInputAndSave, OnInputAndSave, onChange, GlobalStyles.SpawnsetTextInput);
	}
}
