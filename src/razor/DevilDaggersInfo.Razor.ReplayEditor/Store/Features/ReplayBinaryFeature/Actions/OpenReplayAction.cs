using DevilDaggersInfo.Core.Replay;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature.Actions;

public record OpenReplayAction(ReplayBinary<LocalReplayBinaryHeader> ReplayBinary, string Name);
