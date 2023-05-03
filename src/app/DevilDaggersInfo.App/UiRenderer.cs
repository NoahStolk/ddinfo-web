using DevilDaggersInfo.App.Windows;

namespace DevilDaggersInfo.App;

public static class UiRenderer
{
	private static bool _windowShouldClose;

	public static bool WindowShouldClose => _windowShouldClose;

	public static void RenderUi()
	{
		MainWindow.Render(out _windowShouldClose);
	}
}
