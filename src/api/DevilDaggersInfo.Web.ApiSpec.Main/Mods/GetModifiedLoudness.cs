namespace DevilDaggersInfo.Api.Main.Mods;

public record GetModifiedLoudness
{
	public required string AssetName { get; init; }

	public required bool IsProhibited { get; init; }

	public required float DefaultLoudness { get; init; }

	public required float ModifiedLoudness { get; init; }
}
