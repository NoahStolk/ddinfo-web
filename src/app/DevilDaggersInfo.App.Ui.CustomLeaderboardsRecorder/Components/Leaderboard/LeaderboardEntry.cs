namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;

public class LeaderboardEntry : AbstractComponent
{
	private readonly GetCustomEntry _getCustomEntry;

	public LeaderboardEntry(IBounds bounds, GetCustomEntry getCustomEntry)
		: base(bounds)
	{
		_getCustomEntry = getCustomEntry;
	}

	private void WatchInGame()
	{
		AsyncHandler.Run(Inject, () => FetchCustomEntryReplayById.HandleAsync(_getCustomEntry.Id));

		void Inject(GetCustomEntryReplayBuffer? getCustomEntryReplayBuffer)
		{
			if (getCustomEntryReplayBuffer == null)
			{
				Root.Dependencies.NativeDialogService.ReportError("Could not fetch replay.");
				return;
			}

			Root.Dependencies.GameMemoryService.WriteReplayToMemory(getCustomEntryReplayBuffer.Data);
		}
	}

	private void WatchInReplayViewer()
	{
		AsyncHandler.Run(BuildReplayScene, () => FetchCustomEntryReplayById.HandleAsync(_getCustomEntry.Id));

		void BuildReplayScene(GetCustomEntryReplayBuffer? getCustomEntryReplayBuffer)
		{
			if (getCustomEntryReplayBuffer == null)
			{
				Root.Dependencies.NativeDialogService.ReportError("Could not fetch replay.");
				return;
			}

			ReplayBinary<LocalReplayBinaryHeader> replayBinary;
			try
			{
				replayBinary = new(getCustomEntryReplayBuffer.Data);
			}
			catch (Exception ex)
			{
				Root.Dependencies.NativeDialogService.ReportError("Could not parse replay.", ex);
				return;
			}

			StateManager.Dispatch(new SetLayout(Root.Dependencies.CustomLeaderboardsRecorderReplayViewer3dLayout));
			StateManager.Dispatch(new BuildReplayScene(new[] { replayBinary }));
		}
	}
}
