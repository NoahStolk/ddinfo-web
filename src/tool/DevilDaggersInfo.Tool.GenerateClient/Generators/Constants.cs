namespace DevilDaggersInfo.Tool.GenerateClient.Generators;

internal static class Constants
{
	private const string _devRoot = @"C:\Users\NOAH\source\repos\DevilDaggersInfo\src";

	public static readonly string ServerProjectPath = Path.Combine(_devRoot, "web", "DevilDaggersInfo.Web.Server");

	public static readonly string ClientProjectPath = Path.Combine(_devRoot, "web", "DevilDaggersInfo.Web.Client");
	public static readonly string CoreClProjectPath = Path.Combine(_devRoot, "core", "DevilDaggersInfo.Core.CustomLeaderboards");
}
