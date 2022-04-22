namespace DevilDaggersInfo.Web.Shared.Dto.Public.Mods;

public record GetModArchive
{
	public long FileSize { get; init; }

	public long FileSizeExtracted { get; init; }

	public List<GetModBinary> Binaries { get; init; } = null!;
}
