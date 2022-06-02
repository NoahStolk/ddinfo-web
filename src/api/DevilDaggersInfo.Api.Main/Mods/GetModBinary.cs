namespace DevilDaggersInfo.Web.Shared.Dto.Public.Mods;

public record GetModBinary
{
	public string Name { get; init; } = null!;

	public long Size { get; init; }

	public ModBinaryType ModBinaryType { get; init; }

	public bool ContainsProhibitedAssets { get; init; }

	public List<GetModAsset> Assets { get; init; } = new();

	public List<GetModifiedLoudness>? ModifiedLoudness { get; init; }
}
