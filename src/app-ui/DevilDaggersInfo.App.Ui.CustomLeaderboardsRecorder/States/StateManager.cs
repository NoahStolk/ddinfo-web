using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public static class StateManager
{
	private static readonly Dictionary<string, IAction> _actions = new();
	private static readonly Dictionary<string, List<Action>> _subscribedEvents = new();

	public static ActiveSpawnsetState ActiveSpawnsetState { get; private set; } = ActiveSpawnsetState.GetDefault();
	public static LeaderboardListState LeaderboardListState { get; set; } = LeaderboardListState.GetDefault();
	public static LeaderboardState LeaderboardState { get; private set; } = LeaderboardState.GetDefault();
	public static MarkerState MarkerState { get; private set; } = MarkerState.GetDefault();
	public static RecordingState RecordingState { get; private set; } = RecordingState.GetDefault();

	public static void Dispatch(IAction action)
	{
		// Dispatch an action, if it already exists for this action type, overwrite it.
		// TODO: No reflection.
		_actions[action.GetType().Name] = action;
	}

	public static void Subscribe(string actionKey, Action action)
	{
		if (_subscribedEvents.TryGetValue(actionKey, out List<Action>? actions))
			actions.Add(action);
		else
			_subscribedEvents[actionKey] = new() { action };
	}

	public static void ReduceAll()
	{
		foreach (KeyValuePair<string, IAction> action in _actions)
		{
			action.Value.Reduce();
			if (!_subscribedEvents.TryGetValue(action.Key, out List<Action>? actions))
				continue;

			foreach (Action a in actions)
				a.Invoke();
		}

		_actions.Clear();
	}

	public static void SetSelectedCustomLeaderboard(GetCustomLeaderboardForOverview cl)
	{
		LeaderboardListState = LeaderboardListState with
		{
			SelectedCustomLeaderboard = cl,
		};

		AsyncHandler.Run(SetCl, () => FetchCustomLeaderboardById.HandleAsync(cl.Id));

		void SetCl(GetCustomLeaderboard? getCustomLeaderboard)
		{
			LeaderboardState = new(getCustomLeaderboard);

			Root.Game.CustomLeaderboardsRecorderMainLayout.SetCustomLeaderboard();
		}
	}

	public static void SetMarker(long marker)
	{
		MarkerState = new(marker);
	}

	public static void SetRecordingState(RecordingStateType recordingStateType)
	{
		RecordingState = RecordingState with
		{
			RecordingStateType = recordingStateType,
		};
	}

	public static void SetLastSubmission(DateTime lastSubmission)
	{
		RecordingState = RecordingState with
		{
			LastSubmission = lastSubmission,
		};
	}

	public static void SetCurrentPlayerId(int playerId)
	{
		RecordingState = RecordingState with
		{
			CurrentPlayerId = playerId,
		};
	}

	// TODO: Call this when file system watcher is notified.
	public static void RefreshActiveSpawnset()
	{
		if (!File.Exists(UserSettings.ModsSurvivalPath))
		{
			ActiveSpawnsetState = ActiveSpawnsetState.GetDefault();
			return;
		}

		byte[] fileContents = File.ReadAllBytes(UserSettings.ModsSurvivalPath);
		byte[] fileHash = MD5.HashData(fileContents);
		AsyncHandler.Run(Set, () => FetchSpawnsetByHash.HandleAsync(fileHash));

		void Set(GetSpawnsetByHash? getSpawnsetByHash)
		{
			ActiveSpawnsetState = new(getSpawnsetByHash?.Name, fileContents, fileHash);
		}
	}
}
