namespace DevilDaggersInfo.Web.Client.Core.CanvasChart.Data;

public sealed class LineData
{
	public LineData(double x, double y, int index)
	{
		X = x;
		Y = y;
		Index = index;
	}

	public double X { get; }
	public double Y { get; }
	public int Index { get; }
}
