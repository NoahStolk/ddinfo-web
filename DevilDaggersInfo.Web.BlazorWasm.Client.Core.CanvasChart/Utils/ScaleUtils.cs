namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Utils;

public static class ScaleUtils
{
	public static List<double> CalculateScales(double chartSize, double min, double max, double? step, double minimumSizeInPx)
	{
		if (chartSize <= 0 || minimumSizeInPx <= 0 || min == max)
			return new();

		bool tooNarrow = false;
		if (step.HasValue)
		{
			double stepPerc = step.Value / (max - min);
			double stepReal = stepPerc * chartSize;
			tooNarrow = stepReal < minimumSizeInPx;
		}

		List<double> scales;
		if (!step.HasValue || tooNarrow)
		{
			int calculatedCount = (int)Math.Floor(chartSize / minimumSizeInPx);
			double calculatedStep = (max - min) / calculatedCount;
			scales = Enumerable.Range(0, calculatedCount).Select(i => i * calculatedStep + min).Append(max).ToList();
		}
		else
		{
			scales = new List<double>();
			for (double i = min; i <= max; i += step.Value)
				scales.Add(i);
		}

		return scales;
	}
}
