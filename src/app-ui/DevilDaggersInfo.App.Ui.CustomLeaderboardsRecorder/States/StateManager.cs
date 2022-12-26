using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public static class StateManager
{
	// TODO: These should only be able to be set by actions.
	public static ActiveSpawnsetState ActiveSpawnsetState { get; private set; } = ActiveSpawnsetState.GetDefault();
	public static LeaderboardListState LeaderboardListState { get; set; } = LeaderboardListState.GetDefault();
	public static LeaderboardState LeaderboardState { get; set; } = LeaderboardState.GetDefault();
	public static MarkerState MarkerState { get; private set; } = MarkerState.GetDefault();
	public static RecordingState RecordingState { get; private set; } = RecordingState.GetDefault();

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
		// TODO: All.
		Reduce<LoadLeaderboardList>();
		Reduce<SetCategory>();
		Reduce<SetLoading>();
		Reduce<SetPageIndex>();
		Reduce<SetSelectedCustomLeaderboard>();
		Reduce<SetTotalResults>();

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
