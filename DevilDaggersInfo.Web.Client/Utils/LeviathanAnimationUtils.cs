namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class LeviathanAnimationUtils
{
	public static string? GetStyle(int total, int index)
	{
		int offset = (total - index) * 300;
		return offset == 0 ? null : $"animation-delay: -{offset}ms;";
	}
}
