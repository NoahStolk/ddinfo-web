namespace DevilDaggersInfo.Api.Admin.Health;

public record GetResponseTimes
{
	public required Dictionary<string, GetRequestPathEntry> ResponseTimeSummaryByRequestPath { get; set; }

	public required Dictionary<string, Dictionary<int, GetRequestPathEntry>> ResponseTimesByTimeByRequestPath { get; init; }

	public int MinuteInterval { get; init; }
}
