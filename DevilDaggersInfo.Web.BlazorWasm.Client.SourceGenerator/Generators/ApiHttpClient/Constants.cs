using DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Generators.ApiHttpClient.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Generators.ApiHttpClient;

internal static class Constants
{
	public const string ServerProjectName = "DevilDaggersInfo.Web.BlazorWasm.Server";
	public const string SharedProjectName = "DevilDaggersInfo.Web.BlazorWasm.Shared";
	public const string ServerProjectPath = $@"C:\Users\NOAH\source\repos\DevilDaggersInfo\{ServerProjectName}";
	public const string SharedProjectPath = $@"C:\Users\NOAH\source\repos\DevilDaggersInfo\{SharedProjectName}";

	public static IncludedDirectory[] IncludedDirectories { get; } = (IncludedDirectory[])Enum.GetValues(typeof(IncludedDirectory));
	public static ClientType[] ClientTypes { get; } = (ClientType[])Enum.GetValues(typeof(ClientType));
}
