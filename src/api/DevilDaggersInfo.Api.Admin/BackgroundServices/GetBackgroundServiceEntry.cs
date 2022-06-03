namespace DevilDaggersInfo.Api.Admin.BackgroundServices;

public class GetBackgroundServiceEntry
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
