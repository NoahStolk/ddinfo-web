using DevilDaggersInfo.App.Scenes.Base.GameObjects;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Exceptions;
using DevilDaggersInfo.App.Ui.Base.User.Settings;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Config;

public static class ConfigLayout
{
	private static string? _error;
	private static bool _contentInitialized;
	private static string _installationDirectoryInput = string.Empty;

	/// <summary>
	/// Is called on launch, and when the user changes the installation directory.
	/// </summary>
	public static void ValidateInstallation()
	{
		_installationDirectoryInput = UserSettings.Model.DevilDaggersInstallationDirectory;

		try
		{
			ContentManager.Initialize();
		}
		catch (MissingContentException ex)
		{
			_error = ex.Message;
			return;
		}

		UiRenderer.Layout = LayoutType.Main;
		_error = null;

		if (_contentInitialized)
			return;

		Root.GameResources = GameResources.Create(Root.Gl);
		Player.Initialize();
		RaceDagger.Initialize();
		Tile.Initialize();
		Skull4.Initialize();
		Scene.Initialize();
		_contentInitialized = true;
	}

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

		ImGui.SetNextWindowPos(default);
		ImGui.SetNextWindowSize(Constants.LayoutSize);

		ImGui.Begin("Configuration", Constants.LayoutFlags);

		ImGui.Text(text);
		if (!string.IsNullOrWhiteSpace(_error))
			ImGui.TextColored(new(1, 0, 0, 1), _error);

		ImGui.SetCursorPos(new(32, 208));
		if (ImGui.Button("Select installation directory", new(256, 32)))
		{
			string? directory = Root.NativeFileSystemService.SelectDirectory();
			if (directory != null)
				_installationDirectoryInput = directory;
		}

		ImGui.SetCursorPos(new(320, 208));
		ImGui.InputText("##installationDirectoryInput", ref _installationDirectoryInput, 1024, ImGuiInputTextFlags.None);

		ImGui.SetCursorPos(new(512 - 96, 640));
		if (ImGui.Button("Save and continue", new(192, 96)))
		{
			UserSettings.Model = UserSettings.Model with
			{
				DevilDaggersInstallationDirectory = _installationDirectoryInput,
			};

			ValidateInstallation();
		}

		ImGui.End();
	}
}
