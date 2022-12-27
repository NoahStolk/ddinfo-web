using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.States;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.States;
using Warp.NET.Debugging;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement;

public static class StateManager
{
	private static readonly Dictionary<Type, IAction> _actionsToDispatch = new();
	private static readonly Dictionary<Type, List<Action>> _eventHandlers = new();

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
	public static ArenaEditorState ArenaEditorState { get; set; } = ArenaEditorState.GetDefault();
	public static SpawnEditorState SpawnEditorState { get; set; } = SpawnEditorState.GetDefault();
	public static SpawnsetHistoryState SpawnsetHistoryState { get; set; } = SpawnsetHistoryState.GetDefault();
	public static SpawnsetState SpawnsetState { get; set; } = SpawnsetState.GetDefault();

	public static void Subscribe<TAction>(Action eventHandler)
		where TAction : class, IAction
	{
		if (!_eventHandlers.ContainsKey(typeof(TAction)))
			_eventHandlers.Add(typeof(TAction), new());

		_eventHandlers[typeof(TAction)].Add(eventHandler);
	}

	/// <summary>
	/// Dispatches an action, if it already exists for this action type, overwrite it.
	/// </summary>
	/// <param name="action">The instance of the action.</param>
	/// <typeparam name="TAction">The type of the action.</typeparam>
	public static void Dispatch<TAction>(TAction action)
		where TAction : class, IAction
	{
		if (_actionsToDispatch.ContainsKey(typeof(TAction)))
		{
			DebugStack.Add($"Overwriting action {_actionsToDispatch[typeof(TAction)]} with {action}.");
			_actionsToDispatch[typeof(TAction)] = action;
		}
		else
		{
			_actionsToDispatch.Add(typeof(TAction), action);
		}
	}

	public static void ReduceAll()
	{
		// Copy the actions, because new actions can be dispatched while reducing.
		List<KeyValuePair<Type, IAction>> copy = _actionsToDispatch.ToList();
		_actionsToDispatch.Clear();

		// Collect all the event handlers.
		List<Action> allEventHandlers = new();
		foreach (KeyValuePair<Type, IAction> kvp in copy)
		{
			// Reduce the action first.
			kvp.Value.Reduce();

			if (_eventHandlers.TryGetValue(kvp.Key, out List<Action>? eventHandlers))
			{
				// Add the event handlers unless it is already present. An event handler can only be executed once per update (for example, to avoid adding duplicate components).
				allEventHandlers.AddRange(eventHandlers.Where(h => !allEventHandlers.Contains(h)));
			}
		}

		// Execute all the event handlers.
		foreach (Action eventHandler in allEventHandlers)
			eventHandler.Invoke();
	}
}
