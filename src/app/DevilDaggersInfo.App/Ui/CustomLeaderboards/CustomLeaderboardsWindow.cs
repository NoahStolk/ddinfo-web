using DevilDaggersInfo.App.Scenes;
using DevilDaggersInfo.App.Ui.CustomLeaderboards.LeaderboardList;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards;

public static class CustomLeaderboardsWindow
{
	private static int _recordingInterval;

	public static void Update(float delta)
	{
		RecordingChild.Update(delta);

		_recordingInterval++;
		if (_recordingInterval < 5)
			return;

		_recordingInterval = 0;
		if (!RecordingLogic.Scan())
			return;

		//_recordingResultScrollArea.IsActive = gameMemoryInitialized && StateManager.RecordingState.ShowUploadResponse;

		RecordingLogic.Handle();
	}

	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize);
		ImGui.Begin("Custom Leaderboards", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollWithMouse);
		ImGui.PopStyleVar();

		ImGui.BeginChild("LeftRow", new(384, 704));

		StateChild.Render();
		RecordingChild.Render();

		ImGui.EndChild();

		ImGui.SameLine();

		ImGui.BeginChild("RightRow");

		LeaderboardListChild.Render();
		LeaderboardListViewChild.Render();

		ImGui.EndChild();

		ImGui.End();

		if (ImGui.IsKeyPressed(ImGuiKey.Escape) || ImGui.IsKeyPressed((ImGuiKey)526))
		{
			UiRenderer.Layout = LayoutType.Main;
			Scene.SceneType = SceneType.MainMenu;
		}
	}
}
