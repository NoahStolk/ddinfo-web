namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.WorldRecords;

public class GetWorldRecordDataContainer
{
	public List<GetWorldRecordHolder> WorldRecordHolders { get; init; } = null!;

	public Dictionary<GetWorldRecord, GetWorldRecordData> WorldRecordData { get; init; } = null!;
}
