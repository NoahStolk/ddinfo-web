using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Mods;

public record AddMod
{
	[StringLength(64)]
	public required string Name { get; init; }

	public required bool IsHidden { get; init; }

	[StringLength(64)]
	public required string? TrailerUrl { get; init; }

	[StringLength(2048)]
	public required string? HtmlDescription { get; init; }

	public required List<int>? ModTypes { get; init; }

	[StringLength(128)]
	public required string? Url { get; init; }

	public required List<int>? PlayerIds { get; init; }

	[MaxLength(ModConstants.BinaryMaxFiles, ErrorMessage = ModConstants.BinaryMaxFilesErrorMessage)]
	public required List<BinaryData> Binaries { get; init; }

	[MaxLength(ModConstants.ScreenshotMaxFiles, ErrorMessage = ModConstants.ScreenshotMaxFilesErrorMessage)]
	public required Dictionary<string, byte[]> Screenshots { get; init; }
}
