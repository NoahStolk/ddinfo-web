namespace DevilDaggersInfo.Web.Shared.Dto.Admin.BackgroundServices;

public record GetBackgroundServiceEntry
{
	public GetBackgroundServiceEntry(string name, DateTime lastExecuted, TimeSpan interval)
	{
		Name = name;
		LastExecuted = lastExecuted;
		Interval = interval;
	}

	public string Name { get; }

	public DateTime LastExecuted { get; }

	public TimeSpan Interval { get; }
}
