namespace DevilDaggersInfo.Web.Client.Core.CanvasChart.Utils;

public static class LerpUtils
{
	public static double Lerp(double x, double y, double t)
	{
		return x + (y - x) * t;
	}

	public static double RevLerp(double x, double y, double p)
	{
		return (p - x) / (y - x);
	}
}
