namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;

public interface ISurvivalEditorMainLayout : IExtendedLayout
{
	void SetSpawnset(bool hasArenaChanges, bool hasSpawnsChanges, bool hasSettingsChanges);

	void SetHistory();
}
