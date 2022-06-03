namespace DevilDaggersInfo.Api.Main.WorldRecords;

public record GetWorldRecordDataContainer
{
	public List<GetWorldRecordHolder> WorldRecordHolders { get; init; } = null!;

	public List<GetWorldRecord> WorldRecords { get; init; } = null!;
}
