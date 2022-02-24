namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Utils;

public static class TimeUtils
{
	public static string MinutesToTimeString(int totalMinutes)
	{
		totalMinutes %= 1440;

		int hours = totalMinutes / 60;
		int minutes = totalMinutes % 60;
		return $"{hours:00}:{minutes:00}";
	}
}
