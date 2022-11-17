namespace DevilDaggersInfo.Api.Ddae.Mods;

public record GetModArchiveDdae
{
	public long FileSize { get; init; }

	public long FileSizeExtracted { get; init; }

	public required List<GetModBinaryDdae> Binaries { get; init; }
}
