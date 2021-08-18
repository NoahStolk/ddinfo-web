namespace DevilDaggersInfo.Core.Shared.Extensions;

public static class TimeExtensions
{
	public static int To10thMilliTime(this double time) => (int)(time * 10000.0);

	public static int? To10thMilliTime(this double? time) => time.HasValue ? time.Value.To10thMilliTime() : null;

	public static double ToSecondsTime(this int time) => time * 0.0001;

	public static double? ToSecondsTime(this int? time) => time.HasValue ? time.Value.ToSecondsTime() : null;

	public static double ToSecondsTime(this ulong time) => time * 0.0001;

	public static double? ToSecondsTime(this ulong? time) => time.HasValue ? time.Value.ToSecondsTime() : null;
}
