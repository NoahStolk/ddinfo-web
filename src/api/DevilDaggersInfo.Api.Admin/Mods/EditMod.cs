using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Mods;

public record EditMod
{
	[StringLength(64)]
	public string Name { get; init; } = null!;

	public bool IsHidden { get; init; }

	[StringLength(64)]
	public string? TrailerUrl { get; init; }

	[StringLength(2048)]
	public string? HtmlDescription { get; init; }

	public List<int>? ModTypes { get; init; }

	[StringLength(128)]
	public string? Url { get; init; }

	public List<int>? PlayerIds { get; init; }

	public List<string> BinariesToDelete { get; init; } = new();

	[MaxLength(ModConstants.BinaryMaxFiles, ErrorMessage = ModConstants.BinaryMaxFilesErrorMessage)]
	public List<BinaryData> Binaries { get; init; } = new();

	public List<string> ScreenshotsToDelete { get; init; } = new();

	[MaxLength(ModConstants.ScreenshotMaxFiles, ErrorMessage = ModConstants.ScreenshotMaxFilesErrorMessage)]
	public Dictionary<string, byte[]> Screenshots { get; init; } = new();
}
