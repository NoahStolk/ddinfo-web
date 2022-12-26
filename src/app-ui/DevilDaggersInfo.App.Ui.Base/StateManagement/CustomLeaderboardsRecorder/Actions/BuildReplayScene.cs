using DevilDaggersInfo.Core.Replay;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record BuildReplayScene(ReplayBinary<LocalReplayBinaryHeader>[] ReplayBinaries) : IAction<BuildReplayScene>
{
	public void Reduce()
	{
	}
}
