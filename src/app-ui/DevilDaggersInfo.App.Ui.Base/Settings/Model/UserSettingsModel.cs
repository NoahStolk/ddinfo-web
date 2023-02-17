namespace DevilDaggersInfo.App.Ui.Base.Settings.Model;

public record UserSettingsModel
{
	public required string DevilDaggersInstallationDirectory { get; init; }

	public required bool ScaleUiToWindow { get; init; }

	public required bool ShowDebugOutput { get; init; }

	public required bool RenderWhileWindowIsInactive { get; init; }

	public required int MaxFps { get; init; }

	public required float LookSpeed { get; init; }

	public required float FieldOfView { get; init; }

	public static UserSettingsModel Default { get; } = new()
	{
		DevilDaggersInstallationDirectory = string.Empty,
		ScaleUiToWindow = true,
		ShowDebugOutput = false,
		RenderWhileWindowIsInactive = true,
		MaxFps = 300,
		LookSpeed = 20,
		FieldOfView = 2,
	};

	public static int MaxFpsMin => 60;
	public static int MaxFpsMax => 300;
	public static float LookSpeedMin => 1;
	public static float LookSpeedMax => 100;
	public static float FieldOfViewMin => 0.5f;
	public static float FieldOfViewMax => 3.75f;
}
