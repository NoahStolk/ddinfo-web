namespace DevilDaggersInfo.DevUtil.HistoryCsvDump;

internal sealed record ScoreEntry
{
	public required int PlayerId { get; init; }
	public required int Time { get; init; }
}
