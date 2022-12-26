using DevilDaggersInfo.App.Ui.Base.States.Actions;
using DevilDaggersInfo.Core.Replay;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record BuildReplayScene(ReplayBinary<LocalReplayBinaryHeader>[] ReplayBinaries) : IAction<BuildReplayScene>
{
	public void Reduce()
	{
	}
}
