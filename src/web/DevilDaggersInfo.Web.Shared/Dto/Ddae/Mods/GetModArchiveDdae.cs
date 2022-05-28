namespace DevilDaggersInfo.Web.Shared.Dto.Ddae.Mods;

public record GetModArchiveDdae
{
	public long FileSize { get; init; }

	public long FileSizeExtracted { get; init; }

	public List<GetModBinaryDdae> Binaries { get; init; } = null!;
}
