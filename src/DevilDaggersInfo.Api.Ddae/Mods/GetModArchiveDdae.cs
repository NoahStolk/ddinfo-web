namespace DevilDaggersInfo.Api.Ddae.Mods;

public record GetModArchiveDdae
{
	public required long FileSize { get; init; }

	public required long FileSizeExtracted { get; init; }

	public required List<GetModBinaryDdae> Binaries { get; init; }
}
