namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Constants
{
	public static class ModFileConstants
	{
		public const int MaxFileSize = 256 * 1024 * 1024;

		public const long MaxHostingSpace = 5L * 1024 * 1024 * 1024;

		// TODO: Use const string interpolation (requires C# 10).
		public const string MaxFileSizeErrorMessage = "File cannot be larger than 268,435,456 bytes.";
	}
}
