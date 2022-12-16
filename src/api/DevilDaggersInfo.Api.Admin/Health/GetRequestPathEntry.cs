namespace DevilDaggersInfo.Api.Admin.Health;

public record GetRequestPathEntry
{
	public required int RequestCount { get; set; }

	public required double AverageResponseTimeTicks { get; set; }

	public required double MinResponseTimeTicks { get; set; }

	public required double MaxResponseTimeTicks { get; set; }
}
