using System;

namespace DevilDaggersWebsite.Core.Tasks.Cron
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