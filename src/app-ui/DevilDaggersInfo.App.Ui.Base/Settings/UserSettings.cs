using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings.Model;
using System.Text.Json;

namespace DevilDaggersInfo.App.Ui.Base.Settings;

public static class UserSettings
{
	private static readonly string _fileDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ddinfo-tools");
	private static readonly string _filePath = Path.Combine(_fileDirectory, "settings");

	private static UserSettingsModel _model = new();

	public static UserSettingsModel Model
	{
		get => _model;
		set
		{
			_model = value;
			Save();
		}
	}

	public static string ModsDirectory => Path.Combine(_model.DevilDaggersInstallationDirectory, "mods");

	public static string DdDirectory => Path.Combine(_model.DevilDaggersInstallationDirectory, "dd");

	public static string ResDirectory => Path.Combine(_model.DevilDaggersInstallationDirectory, "res");

	public static string ModsSurvivalPath => Path.Combine(ModsDirectory, "survival");

	public static string DdSurvivalPath => Path.Combine(DdDirectory, "survival");

	public static string ResAudioPath => Path.Combine(ResDirectory, "audio");

	public static string ResDdPath => Path.Combine(ResDirectory, "dd");

	public static void Load()
	{
		if (!File.Exists(_filePath))
			return;

		try
		{
			UserSettingsModel? json = JsonSerializer.Deserialize<UserSettingsModel>(File.ReadAllText(_filePath));
			_model = json ?? new()
			{
				DevilDaggersInstallationDirectory = Root.Dependencies.PlatformSpecificValues.DefaultInstallationPath,
				ScaleUiToWindow = true,
			};
		}
		catch (Exception ex)
		{
			Root.Dependencies.Log.Error(ex, "Failed to load user settings.");
		}
	}

	private static void Save()
	{
		Directory.CreateDirectory(_fileDirectory);
		File.WriteAllText(_filePath, JsonSerializer.Serialize(_model));
	}
}
