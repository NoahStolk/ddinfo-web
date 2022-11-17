using DevilDaggersInfo.Types.Core.Wiki;

namespace DevilDaggersInfo.Api.Main.WorldRecords;

public record GetWorldRecord
{
	public DateTime DateTime { get; init; }

	public required GetWorldRecordEntry Entry { get; init; }

	public GameVersion? GameVersion { get; init; }

	public TimeSpan WorldRecordDuration { get; init; }

	public double? WorldRecordImprovement { get; init; }
}
