namespace DevilDaggersInfo.Razor.Core.CanvasChart.Utils;

public static class LerpUtils
{
	public static double Lerp(double x, double y, double t)
		=> x + (y - x) * t;

	public static double RevLerp(double x, double y, double p)
		=> (p - x) / (y - x);
}
