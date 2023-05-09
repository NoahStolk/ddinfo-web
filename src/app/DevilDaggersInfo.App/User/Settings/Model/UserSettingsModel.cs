namespace DevilDaggersInfo.App.User.Settings.Model;

public record UserSettingsModel
{
	public string DevilDaggersInstallationDirectory { get; init; } = string.Empty;
	public bool ShowDebugOutput { get; init; }
	public int MaxFps { get; init; }
	public float LookSpeed { get; init; }
	public int FieldOfView { get; init; }

	public static UserSettingsModel Default { get; } = new()
	{
		DevilDaggersInstallationDirectory = string.Empty,
		ShowDebugOutput = false,
		MaxFps = 300,
		LookSpeed = 20,
		FieldOfView = 90,
	};

	public static int MaxFpsMin => 60;
	public static int MaxFpsMax => 300;
	public static float LookSpeedMin => 1;
	public static float LookSpeedMax => 100;
	public static int FieldOfViewMin => 10;
	public static int FieldOfViewMax => 170;
}
