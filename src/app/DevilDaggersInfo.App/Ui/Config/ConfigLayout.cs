using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Config;

public static class ConfigLayout
{
	//private readonly TextInput _installationDirectoryInput;

	private static string? _error;
	private static bool _contentInitialized;

	// public ConfigLayout()
	// {
	// 	TextButton installationDirectoryButton = new(new PixelBounds(32, 208, 256, 32), PickInstallationDirectory, ButtonStyles.Default, new(Color.White, TextAlign.Middle, FontSize.H12), "Select installation directory");
	// 	NestingContext.Add(installationDirectoryButton);
	//
	// 	_installationDirectoryInput = new(new PixelBounds(320, 208, 640, 32), false, null, null, null, TextInputStyles.Default with { FontSize = FontSize.H16 });
	// 	NestingContext.Add(_installationDirectoryInput);
	//
	// 	NestingContext.Add(new TextButton(new PixelBounds(384, 384, 256, 64), CheckInstallationDirectory, ButtonStyles.Default, new(Color.White, TextAlign.Middle, FontSize.H24), "Save and continue"));
	//
	// 	StateManager.Subscribe<SetLayout>(SetLayout);
	// 	StateManager.Subscribe<UserSettingsLoaded>(ValidateInstallation);
	// }

	// private void PickInstallationDirectory()
	// {
	// 	string? directory = Root.Dependencies.NativeFileSystemService.SelectDirectory();
	// 	if (directory != null)
	// 		_installationDirectoryInput.KeyboardInput.SetText(directory);
	// }
	//
	// private void CheckInstallationDirectory()
	// {
	// 	UserSettings.Model = UserSettings.Model with
	// 	{
	// 		DevilDaggersInstallationDirectory = _installationDirectoryInput.KeyboardInput.Value.ToString(),
	// 	};
	// 	ValidateInstallation();
	// }
	//
	// private void ValidateInstallation()
	// {
	// 	try
	// 	{
	// 		ContentManager.Initialize();
	// 	}
	// 	catch (MissingContentException ex)
	// 	{
	// 		_error = ex.Message;
	// 		return;
	// 	}
	//
	// 	StateManager.Dispatch(new SetLayout(Root.Dependencies.MainLayout));
	// 	_error = null;
	//
	// 	if (_contentInitialized)
	// 		return;
	//
	// 	StateManager.Dispatch(new InitializeContent());
	// 	Player.Initialize();
	// 	RaceDagger.Initialize();
	// 	Tile.Initialize();
	// 	Skull4.Initialize();
	// 	_contentInitialized = true;
	// }

	// private void SetLayout()
	// {
	// 	if (StateManager.LayoutState.CurrentLayout != Root.Dependencies.ConfigLayout)
	// 		return;
	//
	// 	_installationDirectoryInput.KeyboardInput.SetText(UserSettings.Model.DevilDaggersInstallationDirectory);
	// }

	public static void Render()
	{
#pragma warning disable S1075
#if LINUX
		const string examplePath = "/home/{USERNAME}/.local/share/Steam/steamapps/common/devildaggers/";
#elif WINDOWS
		const string examplePath = """C:\Program Files (x86)\Steam\steamapps\common\devildaggers""";
#endif
#pragma warning restore S1075

		const string text = $"""
			Please configure your Devil Daggers installation directory.

			This is the directory containing the executable.

			Example: {examplePath}
			""";

		ImGui.SetNextWindowPos(new(0, 0));
		ImGui.SetNextWindowSize(new(1024, 768));

		ImGui.Begin("Configuration", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus);

		ImGui.Text(text);
		if (!string.IsNullOrWhiteSpace(_error))
			ImGui.TextColored(new(1, 0, 0, 1), _error);

		ImGui.SetCursorPos(new(512 - 96, 640));
		if (ImGui.Button("Save", new(192, 96)))
			UiRenderer.Layout = LayoutType.Main;

		ImGui.End();
	}
}
