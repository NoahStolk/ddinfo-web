using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;

public class DataOptions
{
	public DataOptions(double minX, double? stepX, double maxX, double minY, double? stepY, double maxY)
	{
		MinX = minX;
		StepX = stepX;
		MaxX = maxX;
		MinY = minY;
		StepY = stepY;
		MaxY = maxY;
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
}
