namespace DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;

public record ModifiedLoudnessAssetCacheData
{
	public required string Name { get; init; }
	public required bool IsProhibited { get; init; }
	public required float DefaultLoudness { get; init; }
	public required float ModifiedLoudness { get; init; }
}
