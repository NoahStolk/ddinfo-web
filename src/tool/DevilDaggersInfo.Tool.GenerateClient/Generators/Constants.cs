namespace DevilDaggersInfo.Tool.GenerateClient.Generators;

internal static class Constants
{
	private const string _devRoot = @"C:\Users\NOAH\source\repos\DevilDaggersInfo\src";

	public static readonly string ServerProjectPath = Path.Combine(_devRoot, "web-server", "DevilDaggersInfo.Web.Server");

	public static readonly string ClientProjectPath = Path.Combine(_devRoot, "web-client", "DevilDaggersInfo.Web.Client");
	public static readonly string RazorIamProjectPath = Path.Combine(_devRoot, "razor", "DevilDaggersInfo.Razor.AppManager");
	public static readonly string RazorClProjectPath = Path.Combine(_devRoot, "razor", "DevilDaggersInfo.Razor.CustomLeaderboard");
	public static readonly string RazorReProjectPath = Path.Combine(_devRoot, "razor", "DevilDaggersInfo.Razor.ReplayEditor");
	public static readonly string RazorSeProjectPath = Path.Combine(_devRoot, "razor", "DevilDaggersInfo.Razor.SurvivalEditor");
}
