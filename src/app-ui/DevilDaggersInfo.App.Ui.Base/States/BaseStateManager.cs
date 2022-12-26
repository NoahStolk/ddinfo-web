using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.Base.States.Actions;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.Base.States;

public static class BaseStateManager
{
	public static void Subscribe<TAction>(Action<TAction> eventHandler)
		where TAction : class, IAction<TAction>
	{
		TAction.Subscribe(eventHandler);
	}

	public static void Dispatch<TAction>(TAction action)
		where TAction : class, IAction<TAction>
	{
		// Dispatch an action, if it already exists for this action type, overwrite it.
		TAction.ActionToReduce = action;
	}

	public static void ReduceAll()
	{
		// TODO: Don't do this manually.
		Reduce<SetLayout>();

		static void Reduce<T>()
			where T : class, IAction<T>
		{
			if (T.ActionToReduce == null)
				return;

			T.ActionToReduce.Reduce();
			foreach (Action<T> a in T.EventHandlers)
				a.Invoke(T.ActionToReduce);

			T.ActionToReduce = null;
		}
	}

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

	public static void ToSurvivalEditor3dLayout(SpawnsetBinary spawnset)
	{
		Root.Game.ActiveLayout = Root.Game.SurvivalEditor3dLayout;
		Root.Game.SurvivalEditor3dLayout.BuildScene(spawnset);
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

	public static void ToCustomLeaderboardsRecorderReplayViewer3dLayout(ReplayBinary<LocalReplayBinaryHeader>[] replayBinaries)
	{
		Root.Game.ActiveLayout = Root.Game.CustomLeaderboardsRecorderReplayViewer3dLayout;
		Root.Game.CustomLeaderboardsRecorderReplayViewer3dLayout.BuildScene(replayBinaries);
	}
}
