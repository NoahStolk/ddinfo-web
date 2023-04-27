namespace DevilDaggersInfo.Api.Ddae.Mods;

public record GetModBinaryDdae
{
	public required string Name { get; init; }

	public long Size { get; init; }

	public ModBinaryTypeDdae ModBinaryType { get; init; }
}
