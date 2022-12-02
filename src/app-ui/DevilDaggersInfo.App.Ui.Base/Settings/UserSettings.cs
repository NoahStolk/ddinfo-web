namespace DevilDaggersInfo.App.Ui.Base.Settings;

public static class UserSettings
{
	public static string DevilDaggersInstallationDirectory { get; set; } = @"C:\Program Files (x86)\Steam\steamapps\common\devildaggers";

	public static string ModsSurvivalPath => Path.Combine(DevilDaggersInstallationDirectory, "mods", "survival");

	public static string DdSurvivalPath => Path.Combine(DevilDaggersInstallationDirectory, "dd", "survival");

	public static string ResDdPath => Path.Combine(DevilDaggersInstallationDirectory, "res", "dd");
}
