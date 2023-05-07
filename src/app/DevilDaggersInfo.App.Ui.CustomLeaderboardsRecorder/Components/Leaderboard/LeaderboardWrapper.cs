namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;

public class LeaderboardWrapper : AbstractComponent
{
	public LeaderboardWrapper(IBounds bounds)
		: base(bounds)
	{
		StateManager.Subscribe<SetSuccessfulUpload>(SetCustomLeaderboardFromUploadResponse);
	}

	private void SetCustomLeaderboardFromUploadResponse()
	{
		if (StateManager.UploadResponseState.UploadResponse?.NewSortedEntries == null)
			return;

		_playButton.IsDisabled = false;
		_leaderboardScrollArea.SetContent(StateManager.UploadResponseState.UploadResponse.NewSortedEntries);
	}
}
