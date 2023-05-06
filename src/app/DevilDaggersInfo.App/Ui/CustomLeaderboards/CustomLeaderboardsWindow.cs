using DevilDaggersInfo.App.Scenes;
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

		StateChild.Render();

		RecordingChild.Render();

		ImGui.End();

		if (ImGui.IsKeyPressed(ImGuiKey.Escape) || ImGui.IsKeyPressed((ImGuiKey)526))
		{
			UiRenderer.Layout = LayoutType.Main;
			Scene.SceneType = SceneType.MainMenu;
		}
	}
}
