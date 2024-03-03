using DevilDaggersInfo.Core.Asset;

namespace DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;

public record ModTocEntryCacheData
{
	public required string Name { get; init; }

	public required int Size { get; init; }

	public required AssetType AssetType { get; init; }

	public required bool IsProhibited { get; init; }
}
