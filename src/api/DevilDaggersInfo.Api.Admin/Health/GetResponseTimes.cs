namespace DevilDaggersInfo.Api.Admin.Health;

public record GetResponseTimes
{
	public required Dictionary<string, GetRequestPathEntry> ResponseTimeSummaryByRequestPath { get; set; } // TODO: init

	public required Dictionary<string, Dictionary<int, GetRequestPathEntry>> ResponseTimesByTimeByRequestPath { get; init; }

	public required int MinuteInterval { get; init; }
}
