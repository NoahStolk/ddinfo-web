namespace DevilDaggersInfo.Api.Admin;

public static class ModConstants
{
	public const int BinaryMaxFileSize = 256 * 1024 * 1024;
	public const int BinaryMaxFiles = 80;
	public const string BinaryMaxFilesErrorMessage = "A mod cannot have more than 80 binaries.";

	public const int ScreenshotMaxFileSize = 2 * 1024 * 1024;
	public const int ScreenshotMaxFiles = 16;
	public const string ScreenshotMaxFilesErrorMessage = "A mod cannot have more than 16 screenshots.";
}
