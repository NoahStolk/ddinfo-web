namespace DevilDaggersInfo.Web.ApiSpec.Main.Mods;

public sealed record GetModArchive
{
	public required long FileSize { get; init; }

	public required long FileSizeExtracted { get; init; }

	public required List<GetModBinary> Binaries { get; init; }
}
