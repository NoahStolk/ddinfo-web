namespace DevilDaggersInfo.Tool.GenerateClient.Generators;

internal static class Constants
{
	private const string _devRootWeb = @"C:\Users\NOAH\source\repos\DevilDaggersInfo\src\web";

	public const string ClientProjectName = "DevilDaggersInfo.Web.Client";
	public const string ServerProjectName = "DevilDaggersInfo.Web.Server";
	public const string SharedProjectName = "DevilDaggersInfo.Web.Shared";

	public static readonly string ClientProjectPath = Path.Combine(_devRootWeb, ClientProjectName);
	public static readonly string ServerProjectPath = Path.Combine(_devRootWeb, ServerProjectName);
	public static readonly string SharedProjectPath = Path.Combine(_devRootWeb, SharedProjectName);
}
