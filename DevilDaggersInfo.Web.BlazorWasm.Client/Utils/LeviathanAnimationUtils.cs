namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class LeviathanAnimationUtils
{
	public static int GetOffset(int total, int index) => (total - index) * 300;

	public static string GetStyle(int offset) => $"animation-delay: -{offset}ms;";
}
