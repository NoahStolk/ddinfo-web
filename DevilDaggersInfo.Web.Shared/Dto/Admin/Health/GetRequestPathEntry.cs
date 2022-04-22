namespace DevilDaggersInfo.Web.Shared.Dto.Admin.Health;

public record GetRequestPathEntry
{
	public int RequestCount { get; set; }

	public double AverageResponseTimeTicks { get; set; }

	public double MinResponseTimeTicks { get; set; }

	public double MaxResponseTimeTicks { get; set; }
}
