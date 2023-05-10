using ImGuiNET;
using System.Numerics;

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
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(640, 320));
		ImGui.Begin("Custom Leaderboard Submissions (this session)");
		ImGui.PopStyleVar();

		foreach (UploadResult result in _results.OrderByDescending(ur => ur.SubmittedAt))
			result.Render();

		ImGui.End();
	}
}
