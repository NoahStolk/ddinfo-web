namespace DevilDaggersInfo.Razor.Core.CanvasChart.Utils;

public static class ScaleUtils
{
	public static List<double> CalculateScales(double chartSize, double min, double max, double? step, double minimumSizeInPx, bool allowFractionalScales)
	{
		if (min > max)
			throw new ArgumentOutOfRangeException(nameof(min), $"Parameter {nameof(min)} cannot be larger than parameter {nameof(max)}.");

		if (chartSize <= 0 || minimumSizeInPx <= 0 || min == max)
			return new();

		double range = max - min;

		bool tooNarrow = false;
		if (step.HasValue)
		{
			double stepPerc = step.Value / range;
			double stepReal = stepPerc * chartSize;
			tooNarrow = stepReal < minimumSizeInPx;
		}

		List<double> scales;
		if (!step.HasValue || tooNarrow)
		{
			// Calculate the count based on the size of the chart.
			int calculatedCount = (int)Math.Floor(chartSize / minimumSizeInPx);

			// Calculate the count based on the values.
			int maxCount = (int)range;

			// If we allow fractional scales or the max count is invalid (casting for large doubles might go out of range), we use the calculated count.
			// Otherwise we will use the max count if it is smaller, so we do not end up with scales like { 0, 1, 1, 2, 2 }.
			int stepCount = allowFractionalScales || maxCount <= 0 ? calculatedCount : Math.Min(calculatedCount, maxCount);

			double calculatedStep = range / stepCount;
			scales = Enumerable.Range(0, stepCount).Select(i => i * calculatedStep + min).Append(max).ToList();
		}
		else
		{
			scales = new List<double>();

			double increment = !allowFractionalScales && step.Value < 1 ? 1 : step.Value;
			for (double i = min; i <= max; i += increment)
				scales.Add(i);
		}

		return scales;
	}
}
