using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.SurvivalEditor;

public interface ISurvivalEditorMainLayout : ILayout
{
	void SetSpawnset();

	void SetHistory();
}
