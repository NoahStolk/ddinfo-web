using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards.Results;

public static class CustomLeaderboardResultsWindow
{
	public static List<UploadResult> Results { get; } = new();

	public static void Render()
	{
		ImGui.Begin("Custom Leaderboard Results");

		foreach (UploadResult result in Results.OrderByDescending(ur => ur.SubmittedAt))
			result.Render();

		ImGui.End();
	}
}
