namespace DevilDaggersInfo.App.Ui.Base.Settings.Model;

public record UserSettingsModel
{
	public string DevilDaggersInstallationDirectory { get; init; } = string.Empty;

	public bool ScaleUiToWindow { get; init; }
}
