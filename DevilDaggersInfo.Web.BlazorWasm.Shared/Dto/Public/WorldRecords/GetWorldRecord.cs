using DevilDaggersInfo.Core.Wiki.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.WorldRecords;

public class GetWorldRecord
{
	public DateTime DateTime { get; init; }
	public GetWorldRecordEntry Entry { get; init; } = null!;
	public GameVersion? GameVersion { get; init; }
	public TimeSpan WorldRecordDuration { get; init; }
	public double? WorldRecordImprovement { get; init; }
}
