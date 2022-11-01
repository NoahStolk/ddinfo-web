namespace DevilDaggersInfo.Api.Admin.BackgroundServices;

public record GetBackgroundServiceEntry(string Name, DateTime LastExecuted, TimeSpan Interval);
