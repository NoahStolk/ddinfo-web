namespace DevilDaggersInfo.Web.ApiSpec.Admin.BackgroundServices;

public sealed record GetBackgroundServiceEntry
{
	public required string Name { get; init; }

	public required DateTime LastExecuted { get; init; }

	public required TimeSpan Interval { get; init; }
}
