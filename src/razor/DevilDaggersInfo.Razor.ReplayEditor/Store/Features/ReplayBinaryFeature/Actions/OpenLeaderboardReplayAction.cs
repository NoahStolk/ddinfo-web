using DevilDaggersInfo.Core.Replay;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature.Actions;

public record OpenLeaderboardReplayAction(ReplayBinary<LeaderboardReplayBinaryHeader> ReplayBinary, int PlayerId);
