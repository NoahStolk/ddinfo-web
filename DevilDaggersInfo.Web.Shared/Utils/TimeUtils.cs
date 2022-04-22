namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;

public static class TimeUtils
{
	public static string MinutesToTimeString(int totalMinutes)
	{
		totalMinutes %= 1440;

		int hours = totalMinutes / 60;
		int minutes = totalMinutes % 60;
		return $"{hours:00}:{minutes:00}";
	}

	public static string TicksToTimeString(double ticks)
	{
		if (ticks >= TimeSpan.TicksPerSecond)
			return $"{ticks / TimeSpan.TicksPerSecond:0.00} s";

		if (ticks >= TimeSpan.TicksPerMillisecond)
			return $"{ticks / TimeSpan.TicksPerMillisecond:0.0} ms";

		return $"{ticks / 10f:0} μs";
	}
}
