using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.ReplayEditor;

public static class ReplayEditorWindow
{
	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize);
		ImGui.Begin("Replay Editor", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoScrollWithMouse);
		ImGui.PopStyleVar();

		ReplayEditorMenu.Render();

		ImGui.End();
	}
}
