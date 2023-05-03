using DevilDaggersInfo.App.Ui;
using DevilDaggersInfo.App.Ui.Config;
using DevilDaggersInfo.App.Ui.Main;
using DevilDaggersInfo.App.Ui.SurvivalEditor;

namespace DevilDaggersInfo.App;

public static class UiRenderer
{
	private static bool _windowShouldClose;
	public static bool WindowShouldClose => _windowShouldClose;

	public static LayoutType Layout { get; set; }

	public static void Render()
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
			case LayoutType.SurvivalEditor:
				SurvivalEditorWindow.Render();
				break;
		}

		Modals.Render();
	}
}
