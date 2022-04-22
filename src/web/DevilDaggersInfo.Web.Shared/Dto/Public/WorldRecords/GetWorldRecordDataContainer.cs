namespace DevilDaggersInfo.Web.Shared.Dto.Public.WorldRecords;

public record GetWorldRecordDataContainer
{
	public List<GetWorldRecordHolder> WorldRecordHolders { get; init; } = null!;

	public List<GetWorldRecord> WorldRecords { get; init; } = null!;
}
