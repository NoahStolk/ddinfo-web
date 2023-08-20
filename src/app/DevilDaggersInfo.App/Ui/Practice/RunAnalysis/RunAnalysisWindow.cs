using DevilDaggersInfo.App.Ui.Practice.RunAnalysis.Data;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Practice.RunAnalysis;

public static class RunAnalysisWindow
{
	private static float _recordingTimer;

	public static PracticeStatsData StatsData { get; } = new();

	public static void Update(float delta)
	{
		_recordingTimer += delta;
		if (_recordingTimer < 0.5f)
			return;

		_recordingTimer = 0;
		if (!GameMemoryServiceWrapper.Scan() || !Root.GameMemoryService.IsInitialized)
			return;

		StatsData.Populate();
		GemCollectionChild.Update(StatsData.Statistics);
	}

	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(512, 512));
		if (ImGui.Begin("Run Analysis"))
		{
			SplitsChild.Render();

			// TODO: Fix graphs in GemCollectionChild.
			// GemCollectionChild.Render();

			ImGui.End();
		}

		ImGui.PopStyleVar();
	}
}
