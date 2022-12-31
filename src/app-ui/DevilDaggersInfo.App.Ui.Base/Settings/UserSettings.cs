namespace DevilDaggersInfo.App.Ui.Base.Settings;

public static class UserSettings
{
	private const int _version1 = 1;

	private static readonly string _fileDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ddinfo-tools");
	private static readonly string _filePath = Path.Combine(_fileDirectory, "settings");

	private static string _devilDaggersInstallationDirectory = @"C:\Program Files (x86)\Steam\steamapps\common\devildaggers";

	public static string DevilDaggersInstallationDirectory
	{
		get => _devilDaggersInstallationDirectory;
		set
		{
			_devilDaggersInstallationDirectory = value;
			Save();
		}
	}

	public static string ModsSurvivalPath => Path.Combine(_devilDaggersInstallationDirectory, "mods", "survival");

	public static string DdSurvivalPath => Path.Combine(_devilDaggersInstallationDirectory, "dd", "survival");

	public static string ResAudioPath => Path.Combine(_devilDaggersInstallationDirectory, "res", "audio");

	public static string ResDdPath => Path.Combine(_devilDaggersInstallationDirectory, "res", "dd");

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
