namespace DevilDaggersInfo.Api.Main.Mods;

public record GetModifiedLoudness
{
	public required string AssetName { get; init; }

	public bool IsProhibited { get; init; }

	public float DefaultLoudness { get; init; }

	public float ModifiedLoudness { get; init; }
}
