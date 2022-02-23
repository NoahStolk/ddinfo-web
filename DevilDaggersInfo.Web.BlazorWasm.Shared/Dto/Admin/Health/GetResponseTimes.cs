namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Health;

public class GetResponseTimes
{
	public List<GetRequestPathEntry> ResponseTimesByRequestPath { get; set; } = new();

	public Dictionary<int, List<GetRequestPathEntry>> ResponseTimesByTime { get; init; } = new();
}
