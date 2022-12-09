using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.Core.Replay;

namespace DevilDaggersInfo.App.Ui.Base.States;

public static class LayoutManager
{
	public static void ToConfigLayout()
	{
		Root.Game.ActiveLayout = Root.Game.ConfigLayout;
	}

	public static void ToMainLayout()
	{
		Root.Game.ActiveLayout = Root.Game.MainLayout;
	}

	public static void ToSurvivalEditorMainLayout()
	{
		Root.Game.ActiveLayout = Root.Game.SurvivalEditorMainLayout;
	}

	public static void ToSurvivalEditor3dLayout()
	{
		Root.Game.ActiveLayout = Root.Game.SurvivalEditor3dLayout;
		Root.Game.SurvivalEditor3dLayout.BuildScene();
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

	public static void ToCustomLeaderboardsRecorderMainLayout()
	{
		Root.Game.ActiveLayout = Root.Game.CustomLeaderboardsRecorderMainLayout;
		Root.Game.CustomLeaderboardsRecorderMainLayout.Initialize();
	}

	public static void ToCustomLeaderboardsRecorderReplayViewer3dLayout(ReplayBinary<LocalReplayBinaryHeader>[] replayBinaries)
	{
		Root.Game.ActiveLayout = Root.Game.CustomLeaderboardsRecorderReplayViewer3dLayout;
		Root.Game.CustomLeaderboardsRecorderReplayViewer3dLayout.BuildScene(replayBinaries);
	}
}
