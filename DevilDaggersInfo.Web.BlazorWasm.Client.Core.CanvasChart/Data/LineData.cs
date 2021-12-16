using CanvasCharts.Options;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;

public class LineData
{
	public LineData(double x, double y)
	{
		X = x;
		Y = y;
	}

	public double X { get; }
	public double Y { get; }
}
