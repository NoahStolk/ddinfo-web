namespace DevilDaggersInfo.Web.ApiSpec.Main.WorldRecords;

public sealed record GetWorldRecordDataContainer
{
	public required List<GetWorldRecordHolder> WorldRecordHolders { get; init; }

	public required List<GetWorldRecord> WorldRecords { get; init; }
}
