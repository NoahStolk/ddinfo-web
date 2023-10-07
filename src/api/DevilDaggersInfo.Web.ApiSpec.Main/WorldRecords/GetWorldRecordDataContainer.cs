namespace DevilDaggersInfo.Web.ApiSpec.Main.WorldRecords;

public record GetWorldRecordDataContainer
{
	public required List<GetWorldRecordHolder> WorldRecordHolders { get; init; }

	public required List<GetWorldRecord> WorldRecords { get; init; }
}
