namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;

public class BarData
{
	public BarData(string color, double y, object reference)
	{
		Color = color;
		Y = y;
		Reference = reference;
	}

	public string Color { get; }
	public double Y { get; }

	// TODO: Use index.
	public object Reference { get; }
}
