using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature;

public static class ReplayBinaryReducer
{
	[ReducerMethod]
	public static ReplayBinaryState ReduceOpenBinaryAction(ReplayBinaryState state, OpenReplayAction action)
	{
		return new(action.ReplayBinary, action.Name);
	}

	[ReducerMethod]
	public static ReplayBinaryState ReduceOpenLeaderboardBinaryAction(ReplayBinaryState state, OpenLeaderboardReplayAction action)
	{
		LocalReplayBinaryHeader header = new(0, 0, 0, 0, 0, -1, 0, 0, 0, 0, string.Empty, Array.Empty<byte>());
		ReplayBinary<LocalReplayBinaryHeader> localReplay = new(header, action.ReplayBinary.EventsPerTick);

		return new(localReplay, $"Leaderboard replay from {action.ReplayBinary.Header.Username} ({action.PlayerId}) - Converted to local replay");
	}
}
