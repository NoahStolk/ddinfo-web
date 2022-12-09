using DevilDaggersInfo.Core.Replay;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.CustomLeaderboardsRecorder;

public interface IReplayViewer3dLayout : IExtendedLayout
{
	float CurrentTime { get; }

	void BuildScene(ReplayBinary<LocalReplayBinaryHeader>[] replayBinaries);
}
