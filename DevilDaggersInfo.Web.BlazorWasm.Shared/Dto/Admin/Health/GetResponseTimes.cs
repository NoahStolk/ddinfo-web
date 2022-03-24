namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Health;

public record GetResponseTimes
{
	public Dictionary<string, GetRequestPathEntry> ResponseTimeSummaryByRequestPath { get; set; } = new();

	public Dictionary<string, Dictionary<int, GetRequestPathEntry>> ResponseTimesByTimeByRequestPath { get; init; } = new();

	public int MinuteInterval { get; init; }
}
