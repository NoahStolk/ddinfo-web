using DevilDaggersInfo.App.Ui;
using DevilDaggersInfo.App.Ui.Config;
using DevilDaggersInfo.App.Ui.Global;
using DevilDaggersInfo.App.Ui.Main;
using DevilDaggersInfo.App.Ui.SurvivalEditor;
using DevilDaggersInfo.Core.Versioning;

namespace DevilDaggersInfo.App;

public static class UiRenderer
{
	private static bool _windowShouldClose;
	private static bool _showUpdateAvailable;
	private static AppVersion? _availableVersionNumber;

	public static bool WindowShouldClose => _windowShouldClose;

	public static LayoutType Layout { get; set; }

	public static AppVersion? AvailableVersionNumber
	{
		get => _availableVersionNumber;
		set
		{
			_availableVersionNumber = value;
			if (value != null)
				_showUpdateAvailable = true;
		}
	}

	public static void Render()
	{
		switch (Layout)
		{
			case LayoutType.Main:
				MainLayout.Render(out _windowShouldClose);
				break;
			case LayoutType.Config:
				ConfigLayout.Render();
				break;
			case LayoutType.SurvivalEditor:
				SurvivalEditorLayout.Render();
				break;
		}

		if (AvailableVersionNumber != null)
			UpdateAvailableWindow.Render(ref _showUpdateAvailable, AvailableVersionNumber);
	}
}
