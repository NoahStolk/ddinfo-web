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
	private static readonly Dictionary<Type, IAction> _actions = new();

	public static ActiveSpawnsetState ActiveSpawnsetState { get; private set; } = ActiveSpawnsetState.GetDefault();
	public static LeaderboardListState LeaderboardListState { get; set; } = LeaderboardListState.GetDefault();
	public static LeaderboardState LeaderboardState { get; private set; } = LeaderboardState.GetDefault();
	public static MarkerState MarkerState { get; private set; } = MarkerState.GetDefault();
	public static RecordingState RecordingState { get; private set; } = RecordingState.GetDefault();

	public static void Dispatch(IAction action)
	{
		_actions[action.GetType()] = action;
	}

	public static void Reduce()
	{
		foreach (IAction action in _actions.Values)
			action.Reduce(); // TODO: In case a reducer dispatches more actions, this fails. We may need a separate queue for that.

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
