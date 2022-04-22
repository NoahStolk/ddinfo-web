using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.CanvasChart.Data;

public class BarDataSet
{
	public BarDataSet(List<BarData> data, Func<BarDataSet, int, List<MarkupString>> toHighlighterValue)
	{
		Data = data;
		ToHighlighterValue = toHighlighterValue;
	}

	public List<BarData> Data { get; }
	public Func<BarDataSet, int, List<MarkupString>> ToHighlighterValue { get; set; }
}
