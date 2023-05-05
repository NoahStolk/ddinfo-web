using DevilDaggersInfo.App.Ui.SurvivalEditor.Arena;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SurvivalEditorWindow
{
	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize);
		ImGui.Begin("Survival Editor", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoScrollWithMouse);
		ImGui.PopStyleVar();

		SurvivalEditorMenu.Render();

		SpawnsChild.Render();

		ImGui.SameLine();
		ArenaChild.Render();

		ImGui.SameLine();
		SettingsChild.Render();

		ImGui.SameLine();
		HistoryChild.Render();

		ImGui.End();
	}

	/*
	public void Update()
	{
		if (!Input.IsCtrlHeld())
			return;

		Keys? key = _keySubmitter.GetKey();
		if (key.HasValue)
		{
			if (key == Keys.Z && StateManager.SpawnsetHistoryState.CurrentIndex > 0)
				StateManager.Dispatch(new SetSpawnsetHistoryIndex(StateManager.SpawnsetHistoryState.CurrentIndex - 1));
			else if (key == Keys.Y && StateManager.SpawnsetHistoryState.CurrentIndex < StateManager.SpawnsetHistoryState.History.Count - 1)
				StateManager.Dispatch(new SetSpawnsetHistoryIndex(StateManager.SpawnsetHistoryState.CurrentIndex + 1));
		}
	}
	 */
}
