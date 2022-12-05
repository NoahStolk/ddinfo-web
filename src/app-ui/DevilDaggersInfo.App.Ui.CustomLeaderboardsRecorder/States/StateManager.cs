using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.Types.Web;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public static class StateManager
{
	public static ActiveSpawnsetState ActiveSpawnsetState { get; private set; } = ActiveSpawnsetState.GetDefault();
	public static LeaderboardListState LeaderboardListState { get; private set; } = LeaderboardListState.GetDefault();
	public static MarkerState MarkerState { get; private set; } = MarkerState.GetDefault();
	public static RecordingState RecordingState { get; private set; } = RecordingState.GetDefault();

	public static void SetPageIndex(int pageIndex)
	{
		LeaderboardListState = LeaderboardListState with
		{
			PageIndex = pageIndex,
		};
	}

	public static void SetCategory(CustomLeaderboardCategory category)
	{
		LeaderboardListState = LeaderboardListState with
		{
			Category = category,
		};
	}

	public static void SetLoading(bool isLoading)
	{
		LeaderboardListState = LeaderboardListState with
		{
			IsLoading = isLoading,
		};
	}

	public static void SetTotalResults(int totalResults)
	{
		int newMaxPageIndex = (int)Math.Ceiling((totalResults + 1) / (float)LeaderboardListState.PageSize) - 1;
		LeaderboardListState = LeaderboardListState with
		{
			MaxPageIndex = newMaxPageIndex,
			TotalResults = totalResults,
			PageIndex = Math.Clamp(LeaderboardListState.PageIndex, 0, newMaxPageIndex),
		};
	}

	public static void SetSelectedCustomLeaderboard(GetCustomLeaderboardForOverview cl)
	{
		LeaderboardListState = LeaderboardListState with
		{
			SelectedCustomLeaderboard = cl,
		};
	}

	public static void SetMarker(long marker)
	{
		MarkerState = new(marker);
	}

	public static void SetRecordingState(RecordingStateType recordingStateType)
	{
		RecordingState = new(recordingStateType);
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
