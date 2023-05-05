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
}
