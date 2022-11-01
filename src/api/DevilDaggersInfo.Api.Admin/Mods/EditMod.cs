using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Mods;

public record EditMod
{
	[StringLength(64)]
	public required string Name { get; init; }

	public bool IsHidden { get; init; }

	[StringLength(64)]
	public string? TrailerUrl { get; init; }

	[StringLength(2048)]
	public string? HtmlDescription { get; init; }

	public List<int>? ModTypes { get; init; }

	[StringLength(128)]
	public string? Url { get; init; }

	public List<int>? PlayerIds { get; init; }

	public required List<string> BinariesToDelete { get; init; }

	[MaxLength(ModConstants.BinaryMaxFiles, ErrorMessage = ModConstants.BinaryMaxFilesErrorMessage)]
	public required List<BinaryData> Binaries { get; init; }

	public required List<string> ScreenshotsToDelete { get; init; }

	[MaxLength(ModConstants.ScreenshotMaxFiles, ErrorMessage = ModConstants.ScreenshotMaxFilesErrorMessage)]
	public required Dictionary<string, byte[]> Screenshots { get; init; }
}
