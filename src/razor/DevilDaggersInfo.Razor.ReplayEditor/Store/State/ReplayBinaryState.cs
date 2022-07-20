using DevilDaggersInfo.Core.Replay;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.State;

public record ReplayBinaryState(ReplayBinary<LocalReplayBinaryHeader> ReplayBinary, string Name);
