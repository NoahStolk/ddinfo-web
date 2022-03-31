namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient;

internal static class Constants
{
	private const string _devRoot = @"C:\Users\NOAH\source\repos\DevilDaggersInfo";

	public const string ClientProjectName = "DevilDaggersInfo.Web.BlazorWasm.Client";
	public const string ServerProjectName = "DevilDaggersInfo.Web.BlazorWasm.Server";
	public const string SharedProjectName = "DevilDaggersInfo.Web.BlazorWasm.Shared";

	public static readonly string ClientProjectPath = Path.Combine(_devRoot, ClientProjectName);
	public static readonly string ServerProjectPath = Path.Combine(_devRoot, ServerProjectName);
	public static readonly string SharedProjectPath = Path.Combine(_devRoot, SharedProjectName);
}
