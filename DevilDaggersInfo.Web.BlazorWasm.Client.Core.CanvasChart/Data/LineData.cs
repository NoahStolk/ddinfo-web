namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;

public class LineData
{
	public LineData(double x, double y, object reference)
	{
		X = x;
		Y = y;
		Reference = reference;
	}

	public double X { get; }
	public double Y { get; }
	public object Reference { get; }
}
