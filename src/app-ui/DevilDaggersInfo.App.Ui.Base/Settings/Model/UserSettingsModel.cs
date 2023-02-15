namespace DevilDaggersInfo.App.Ui.Base.Settings.Model;

public record UserSettingsModel
{
	public required string DevilDaggersInstallationDirectory { get; init; }

	public required bool ScaleUiToWindow { get; init; }

	public required bool ShowDebugOutput { get; init; }

	public required bool RenderWhileWindowIsInactive { get; init; }

	public required bool AlwaysRecordMemoryForCustomLeaderboards { get; init; }

	public required int MaxFps { get; init; }

	public static UserSettingsModel Default { get; } = new()
	{
		DevilDaggersInstallationDirectory = string.Empty,
		ScaleUiToWindow = true,
		ShowDebugOutput = false,
		RenderWhileWindowIsInactive = true,
		AlwaysRecordMemoryForCustomLeaderboards = false,
		MaxFps = 300,
	};
}
