namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.BarChart;

public class BarChartDataOptions
{
	public BarChartDataOptions(double minY, double? stepY, double maxY)
	{
		MinY = minY;
		StepY = stepY;
		MaxY = maxY;
	}

	public double MinY { get; }

	/// <summary>
	/// Defines the size of the step for the Y scale. If the value is <see langword="null" />, or the steps are too small according to the grid configuration, then this property is ignored and the scales will be calculated according to the grid configuration.
	/// </summary>
	public double? StepY { get; }

	public double MaxY { get; }

	public static BarChartDataOptions Default { get; } = new(0, 1, 10);
}
