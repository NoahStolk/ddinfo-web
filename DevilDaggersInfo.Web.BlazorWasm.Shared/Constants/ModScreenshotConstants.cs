namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Constants
{
	public static class ModScreenshotConstants
	{
		public const int MaxFileSize = 1024 * 1024;

		public const int MaxScreenshots = 10;

		// TODO: Use const string interpolation (requires C# 10).
		public const string MaxFileSizeErrorMessage = "File cannot be larger than 1,048,576 bytes.";
	}
}
