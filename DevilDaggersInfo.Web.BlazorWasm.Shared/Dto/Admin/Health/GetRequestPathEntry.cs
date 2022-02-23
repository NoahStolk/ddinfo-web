namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Health;

public record GetRequestPathEntry
{
	public string RequestPath { get; init; } = null!;

	public int RequestCount { get; init; }

	public double AverageResponseTimeTicks { get; init; }

	public double MinResponseTimeTicks { get; init; }

	public double MaxResponseTimeTicks { get; init; }
}
