namespace DevilDaggersInfo.Common.Utils;

public static class TimeUtils
{
	public static string MinutesToTimeString(int totalMinutes)
	{
		totalMinutes %= 1440;

		int hours = totalMinutes / 60;
		int minutes = totalMinutes % 60;
		return $"{hours:00}:{minutes:00}";
	}

	public static string TicksToTimeString(double ticks) => ticks switch
	{
		>= TimeSpan.TicksPerSecond => $"{ticks / TimeSpan.TicksPerSecond:0.00} s",
		>= TimeSpan.TicksPerMillisecond => $"{ticks / TimeSpan.TicksPerMillisecond:0.0} ms",
		_ => $"{ticks / 10f:0} μs",
	};
}
