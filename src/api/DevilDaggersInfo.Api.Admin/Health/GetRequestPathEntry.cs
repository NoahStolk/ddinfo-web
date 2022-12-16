namespace DevilDaggersInfo.Api.Admin.Health;

// TODO: Immutable.
public record GetRequestPathEntry
{
	public int RequestCount { get; set; } // TODO: init

	public double AverageResponseTimeTicks { get; set; }

	public double MinResponseTimeTicks { get; set; }

	public double MaxResponseTimeTicks { get; set; }
}
