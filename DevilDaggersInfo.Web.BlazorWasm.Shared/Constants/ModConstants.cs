namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;

public static class ModConstants
{
	public const int BinaryMaxFileSize = 256 * 1024 * 1024;
	public const string BinaryMaxFileSizeErrorMessage = "Mod binary cannot be larger than 268,435,456 bytes.";
	public const int BinaryMaxFiles = 80;
	public const string BinaryMaxFilesErrorMessage = "A mod cannot have more than 80 binaries.";
	public const long BinaryMaxHostingSpace = 5L * 1024 * 1024 * 1024;

	public const int ScreenshotMaxFileSize = 1024 * 1024;
	public const string ScreenshotMaxFileSizeErrorMessage = "Mod screenshot cannot be larger than 1,048,576 bytes.";
	public const int ScreenshotMaxFiles = 16;
	public const string ScreenshotMaxFilesErrorMessage = "A mod cannot have more than 16 screenshots.";
}
