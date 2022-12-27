using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.States;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.States;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement;

public static class StateManager
{
	// TODO: These should only be able to be set by actions.
	// Base states.
	public static LayoutState LayoutState { get; set; } = LayoutState.GetDefault();

	// Custom leaderboards recorder states.
	public static ActiveSpawnsetState ActiveSpawnsetState { get; set; } = ActiveSpawnsetState.GetDefault();
	public static LeaderboardListState LeaderboardListState { get; set; } = LeaderboardListState.GetDefault();
	public static LeaderboardState LeaderboardState { get; set; } = LeaderboardState.GetDefault();
	public static MarkerState MarkerState { get; set; } = MarkerState.GetDefault();
	public static RecordingState RecordingState { get; set; } = RecordingState.GetDefault();
	public static ReplaySceneState ReplaySceneState { get; set; } = ReplaySceneState.GetDefault();

	// Survival editor states.
	public static SpawnsetState SpawnsetState { get; set; } = SpawnsetState.GetDefault();
	public static ArenaEditorState ArenaEditorState { get; set; } = ArenaEditorState.GetDefault();
	public static SpawnEditorState SpawnEditorState { get; set; } = SpawnEditorState.GetDefault();
	public static SpawnsetHistoryState SpawnsetHistoryState { get; set; } = SpawnsetHistoryState.GetDefault();

	public static void Subscribe<TAction>(Action eventHandler)
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

		// Base actions.
		Reduce<InitializeContent>();
		Reduce<SetLayout>();
		Reduce<ValidateInstallation>();

		// Custom leaderboards recorder actions.
		Reduce<BuildReplayScene>();
		Reduce<LoadLeaderboardList>();
		Reduce<SetActiveSpawnset>();
		Reduce<SetCategory>();
		Reduce<SetCurrentPlayerId>();
		Reduce<SetLastSubmission>();
		Reduce<SetLoading>();
		Reduce<SetMarker>();
		Reduce<SetPageIndex>();
		Reduce<SetRecordingState>();
		Reduce<SetSelectedCustomLeaderboard>();
		Reduce<SetTotalResults>();
		Reduce<UpdateDisplayedCustomLeaderboard>();

		// Survival editor actions.
		Reduce<ClearSpawnSelections>();
		Reduce<DeselectSpawn>();
		Reduce<LoadSpawnset>();
		Reduce<LoadSpawnsetFromHistory>();
		Reduce<ReplaceCurrentlyActiveSpawnset>();
		Reduce<SelectSpawn>();
		Reduce<SetArenaBucketTolerance>();
		Reduce<SetArenaBucketVoidHeight>();
		Reduce<SetArenaSelectedHeight>();
		Reduce<SetArenaTool>();
		Reduce<SetHistory>();
		Reduce<SetSpawnsetHistoryIndex>();
		Reduce<ToggleSpawnSelection>();
		Reduce<UpdateArena>();
		Reduce<UpdateRaceDaggerPosition>();
		Reduce<UpdateSpawns>();
		Reduce<UpdateSpawnsetSetting>();

		static void Reduce<T>()
			where T : class, IAction<T>
		{
			if (T.ActionToReduce == null)
				return;

			T.ActionToReduce.Reduce();
			foreach (Action eventHandler in T.EventHandlers)
				eventHandler.Invoke();

			T.ActionToReduce = null;
		}
	}
}
