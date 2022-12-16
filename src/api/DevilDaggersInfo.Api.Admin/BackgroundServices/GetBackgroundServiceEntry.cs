namespace DevilDaggersInfo.Api.Admin.BackgroundServices;

public record GetBackgroundServiceEntry
{
	public required string Name { get; init; }

	public required DateTime LastExecuted { get; init; }

	public required TimeSpan Interval { get; init; }
}
