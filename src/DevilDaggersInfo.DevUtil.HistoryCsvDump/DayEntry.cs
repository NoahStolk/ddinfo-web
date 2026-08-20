namespace DevilDaggersInfo.DevUtil.HistoryCsvDump;

internal sealed record DayEntry
{
	public required int DaysSinceRelease { get; init; }
	public required List<ScoreEntry> ScoreEntries { get; init; }
}
