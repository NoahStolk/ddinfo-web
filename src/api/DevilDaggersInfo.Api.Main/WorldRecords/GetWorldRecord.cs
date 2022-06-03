using DevilDaggersInfo.Api.Main.GameVersions;

namespace DevilDaggersInfo.Api.Main.WorldRecords;

public record GetWorldRecord
{
	public DateTime DateTime { get; init; }

	public GetWorldRecordEntry Entry { get; init; } = null!;

	public GameVersion? GameVersion { get; init; }

	public TimeSpan WorldRecordDuration { get; init; }

	public double? WorldRecordImprovement { get; init; }
}
