using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;

public class BarDataSet
{
	public BarDataSet(List<BarData> data, Func<BarDataSet, BarData, List<MarkupString>> toHighlighterValue)
	{
		Data = data;
		ToHighlighterValue = toHighlighterValue;
	}

	public List<BarData> Data { get; }
	public Func<BarDataSet, BarData, List<MarkupString>> ToHighlighterValue { get; set; }
}
