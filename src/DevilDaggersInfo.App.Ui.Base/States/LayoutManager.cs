using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;

namespace DevilDaggersInfo.App.Ui.Base.States;

public static class LayoutManager
{
	public static void ToMainLayout()
	{
		Root.Game.ActiveLayout = Root.Game.MainLayout;
	}

	public static void ToSurvivalEditorMainLayout()
	{
		Root.Game.ActiveLayout = Root.Game.SurvivalEditorMainLayout;
	}

	public static void ToSurvivalEditorOpenLayout()
	{
		Root.Game.SurvivalEditorOpenLayout.SetComponentsFromPath(UserSettings.DevilDaggersInstallationDirectory);
		Root.Game.ActiveLayout = Root.Game.SurvivalEditorOpenLayout;
	}

	public static void ToSurvivalEditorSaveLayout()
	{
		Root.Game.SurvivalEditorSaveLayout.SetComponentsFromPath(UserSettings.DevilDaggersInstallationDirectory);
		Root.Game.ActiveLayout = Root.Game.SurvivalEditorSaveLayout;
	}
}
