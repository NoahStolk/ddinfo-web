using DevilDaggersInfo.App.Ui.Base.DependencyPattern;

namespace DevilDaggersInfo.App.Ui.Base.Settings;

public static class UserSettings
{
	private const int _version1 = 1;

	private static readonly string _fileDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ddinfo-tools");
	private static readonly string _filePath = Path.Combine(_fileDirectory, "settings");

	private static string _devilDaggersInstallationDirectory = Root.Dependencies.PlatformSpecificValues.DefaultInstallationPath;

	public static string DevilDaggersInstallationDirectory
	{
		get => _devilDaggersInstallationDirectory;
		set
		{
			_devilDaggersInstallationDirectory = value;
			Save();
		}
	}

	public static string ModsDirectory => Path.Combine(_devilDaggersInstallationDirectory, "mods");

	public static string DdDirectory => Path.Combine(_devilDaggersInstallationDirectory, "dd");

	public static string ResDirectory => Path.Combine(_devilDaggersInstallationDirectory, "res");

	public static string ModsSurvivalPath => Path.Combine(ModsDirectory, "survival");

	public static string DdSurvivalPath => Path.Combine(DdDirectory, "survival");

	public static string ResAudioPath => Path.Combine(ResDirectory, "audio");

	public static string ResDdPath => Path.Combine(ResDirectory, "dd");

	public static void Load()
	{
		if (!File.Exists(_filePath))
			return;

		using FileStream fs = new(_filePath, FileMode.Open);
		using BinaryReader br = new(fs);
		int version = br.ReadInt32();
		if (version != _version1)
			return;

		_devilDaggersInstallationDirectory = br.ReadString();
	}

	private static void Save()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(_version1);
		bw.Write(_devilDaggersInstallationDirectory);

		Directory.CreateDirectory(_fileDirectory);
		File.WriteAllBytes(_filePath, ms.ToArray());
	}
}
