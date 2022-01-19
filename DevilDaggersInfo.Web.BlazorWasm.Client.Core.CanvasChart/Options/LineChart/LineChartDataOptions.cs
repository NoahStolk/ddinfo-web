namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;

public class LineChartDataOptions
{
	public LineChartDataOptions(double minX, double? stepX, double maxX, double minY, double? stepY, double maxY, bool allowFractionalScales = false, bool reverseY = false)
	{
		MinX = minX;
		StepX = stepX;
		MaxX = maxX;
		MinY = minY;
		StepY = stepY;
		MaxY = maxY;
		AllowFractionalScales = allowFractionalScales;
		ReverseY = reverseY;
	}

	public double MinX { get; }

	/// <summary>
	/// Defines the size of the step for the X scale. If the value is <see langword="null" />, or the steps are too small according to the grid configuration, then this property is ignored and the scales will be calculated according to the grid configuration.
	/// </summary>
	public double? StepX { get; }

	public double MaxX { get; }

	public double MinY { get; }

	/// <summary>
	/// Defines the size of the step for the Y scale. If the value is <see langword="null" />, or the steps are too small according to the grid configuration, then this property is ignored and the scales will be calculated according to the grid configuration.
	/// </summary>
	public double? StepY { get; }

	public double MaxY { get; }

	/// <summary>
	/// By default, fractional scales will not be rendered. For values like percentages however, fractional scales are required. Use this property to override that setting.
	/// </summary>
	public bool AllowFractionalScales { get; }

	public bool ReverseY { get; }
}
