using DevilDaggersInfo.App.Ui.Base.States.Actions;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public static class StateManager
{
	// TODO: These should only be able to be set by actions.
	public static ActiveSpawnsetState ActiveSpawnsetState { get; set; } = ActiveSpawnsetState.GetDefault();
	public static LeaderboardListState LeaderboardListState { get; set; } = LeaderboardListState.GetDefault();
	public static LeaderboardState LeaderboardState { get; set; } = LeaderboardState.GetDefault();
	public static MarkerState MarkerState { get; set; } = MarkerState.GetDefault();
	public static RecordingState RecordingState { get; set; } = RecordingState.GetDefault();

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
}
