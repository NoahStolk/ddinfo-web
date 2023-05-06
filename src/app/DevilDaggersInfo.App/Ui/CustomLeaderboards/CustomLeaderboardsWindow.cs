using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards;

public static class CustomLeaderboardsWindow
{
	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize);
		ImGui.Begin("Custom Leaderboards", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollWithMouse);
		ImGui.PopStyleVar();

		StateChild.Render();

		ImGui.End();
	}
}
