namespace DevilDaggersWebsite.Razor.Utils
{
	public static class ToolRuntimeUtils
	{
		public const string RuntimeNameDotNetCore31 = ".NET Runtime 3.1.x";
		public const string DesktopRuntimeNameDotNetCore31 = ".NET Desktop Runtime 3.1.x";

		public const string RuntimeNameDotNet50 = ".NET Runtime 5.0.x";
		public const string DesktopRuntimeNameDotNet50 = ".NET Desktop Runtime 5.0.x";

		public static string GetRuntimeUrl(string version)
			=> $"https://dotnet.microsoft.com/download/dotnet/{version}";
	}
}
