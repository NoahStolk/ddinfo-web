using DevilDaggersInfo.App.Ui;
using DevilDaggersInfo.App.Ui.Config;
using DevilDaggersInfo.App.Ui.Global;
using DevilDaggersInfo.App.Ui.Main;
using DevilDaggersInfo.App.Ui.SurvivalEditor;
using System.Diagnostics.CodeAnalysis;

namespace DevilDaggersInfo.App;

[SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Readability")]
public static class UiRenderer
{
	private static bool _windowShouldClose;
	public static bool WindowShouldClose => _windowShouldClose;

	private static bool _showError;
	private static ErrorWindow.Error? _error;
	public static ErrorWindow.Error? Error
	{
		get => _error;
		set
		{
			_error = value;
			if (value != null)
				_showError = true;
		}
	}

	private static bool _showMessage;
	private static MessageWindow.Message? _message;
	public static MessageWindow.Message? Message
	{
		get => _message;
		set
		{
			_message = value;
			if (value != null)
				_showMessage = true;
		}
	}

	private static bool _showUpdateAvailable;
	private static UpdateAvailableWindow.UpdateAvailable? _availableVersionNumber;
	public static UpdateAvailableWindow.UpdateAvailable? AvailableVersionNumber
	{
		get => _availableVersionNumber;
		set
		{
			_availableVersionNumber = value;
			if (value != null)
				_showUpdateAvailable = true;
		}
	}

	public static LayoutType Layout { get; set; }

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

		if (Error != null)
			ErrorWindow.Render(ref _showError, Error);

		if (Message != null)
			MessageWindow.Render(ref _showMessage, Message);
	}
}
