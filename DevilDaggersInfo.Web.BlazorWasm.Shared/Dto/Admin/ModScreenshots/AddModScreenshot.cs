namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.ModScreenshots;

public class AddModScreenshot
{
	public string ModName { get; set; } = null!;

	[MaxLength(ModScreenshotConstants.MaxFileSize, ErrorMessage = ModScreenshotConstants.MaxFileSizeErrorMessage)]
	public byte[] FileContents { get; set; } = null!;
}
