using DevilDaggersInfo.App.Ui;
using DevilDaggersInfo.App.Ui.Config;
using DevilDaggersInfo.App.Ui.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.CustomLeaderboards.Results;
using DevilDaggersInfo.App.Ui.Main;
using DevilDaggersInfo.App.Ui.ReplayEditor;
using DevilDaggersInfo.App.Ui.SpawnsetEditor;
using DevilDaggersInfo.App.User.Settings;

namespace DevilDaggersInfo.App;

public static class UiRenderer
{
	private static bool _windowShouldClose;
	public static bool WindowShouldClose => _windowShouldClose;

	public static LayoutType Layout { get; set; }

	public static void Render(float delta)
	{
		//ImGuiNET.ImGui.ShowUserGuide();
		//ImGuiNET.ImGui.ShowDemoWindow();

		switch (Layout)
		{
			case LayoutType.Main:
				MainLayout.Render(out _windowShouldClose);
				break;
			case LayoutType.Config:
				ConfigLayout.Render();
				break;
			case LayoutType.SpawnsetEditor:
				SpawnsetEditorWindow.Render();
				SpawnsetEditor3DWindow.Render();
				break;
			case LayoutType.CustomLeaderboards:
				CustomLeaderboardsWindow.Update(delta);
				CustomLeaderboardsWindow.Render();
				CustomLeaderboardResultsWindow.Render();
				break;
			case LayoutType.ReplayEditor:
				ReplayEditorWindow.Update(delta);
				ReplayEditorWindow.Render();
				ReplayEditor3DWindow.Render();
				break;
		}

		if (UserSettings.Model.ShowDebugOutput)
			DebugWindow.Render();

		Modals.Render();
	}
}
