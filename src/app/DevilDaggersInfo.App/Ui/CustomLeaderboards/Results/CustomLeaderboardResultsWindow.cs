using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards.Results;

public static class CustomLeaderboardResultsWindow
{
	private static readonly List<UploadResult> _results = new();

	public static void AddResult(UploadResult newResult)
	{
		foreach (UploadResult result in _results)
			result.IsExpanded = false;

		_results.Add(newResult);
	}

	public static void Render()
	{
		ImGui.Begin("Custom Leaderboard Results");

		foreach (UploadResult result in _results.OrderByDescending(ur => ur.SubmittedAt))
			result.Render();

		ImGui.End();
	}
}
