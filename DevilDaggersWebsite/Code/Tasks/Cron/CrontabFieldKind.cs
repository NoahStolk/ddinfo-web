using System;

namespace DevilDaggersWebsite.Code.Tasks.Cron
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