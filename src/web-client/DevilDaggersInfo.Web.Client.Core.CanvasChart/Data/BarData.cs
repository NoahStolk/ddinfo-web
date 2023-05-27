namespace DevilDaggersInfo.Web.Client.Core.CanvasChart.Data;

public class BarData
{
	public BarData(string color, double y, int index)
	{
		Color = color;
		Y = y;
		Index = index;
	}

	public string Color { get; }
	public double Y { get; }
	public int Index { get; }
}
