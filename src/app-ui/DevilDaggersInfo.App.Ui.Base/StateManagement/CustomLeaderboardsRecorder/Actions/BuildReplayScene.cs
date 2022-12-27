using DevilDaggersInfo.Core.Replay;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record BuildReplayScene(ReplayBinary<LocalReplayBinaryHeader>[] ReplayBinaries) : IAction
{
	public void Reduce()
	{
		StateManager.ReplaySceneState = new(ReplayBinaries);
	}
}
