namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Mods;

public class EditMod
{
	[StringLength(64)]
	public string Name { get; set; } = null!;

	public bool IsHidden { get; set; }

	[StringLength(64)]
	public string? TrailerUrl { get; set; }

	[StringLength(2048)]
	public string? HtmlDescription { get; set; }

	public List<int>? ModTypes { get; set; }

	[StringLength(128)]
	public string? Url { get; set; }

	public List<int>? PlayerIds { get; set; }

	public List<string> BinariesToDelete { get; set; } = new();

	[MaxLength(ModConstants.BinaryMaxFiles, ErrorMessage = ModConstants.BinaryMaxFilesErrorMessage)]
	public Dictionary<string, byte[]> Binaries { get; set; } = new();

	public List<string> ScreenshotsToDelete { get; set; } = new();

	[MaxLength(ModConstants.ScreenshotMaxFiles, ErrorMessage = ModConstants.ScreenshotMaxFilesErrorMessage)]
	public Dictionary<string, byte[]> Screenshots { get; set; } = new();
}
