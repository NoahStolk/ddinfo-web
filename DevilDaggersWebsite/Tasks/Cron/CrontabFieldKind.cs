using System;

namespace DevilDaggersWebsite.Tasks.Cron
{
	[Serializable]
	public enum CrontabFieldKind
	{
		Minute,
		Hour,
		Day,
		Month,
		DayOfWeek,
	}
}