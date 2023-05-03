using DevilDaggersInfo.App.Ui.Main;

namespace DevilDaggersInfo.App;

public static class UiRenderer
{
	private static bool _windowShouldClose;

	public static bool WindowShouldClose => _windowShouldClose;

	public static void RenderUi()
	{
		MainLayout.Render(out _windowShouldClose);
	}
}
