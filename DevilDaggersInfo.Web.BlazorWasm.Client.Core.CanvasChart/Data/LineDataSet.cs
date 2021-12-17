using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;

public class LineDataSet
{
	public LineDataSet(string color, bool prependStart, bool appendEnd, bool isSteppedLine, List<LineData> data, Func<LineDataSet, LineData, List<MarkupString>> toHighlighterValue)
	{
		Color = color;
		PrependStart = prependStart;
		AppendEnd = appendEnd;
		IsSteppedLine = isSteppedLine;
		Data = data;
		ToHighlighterValue = toHighlighterValue;
	}

	public string Color { get; }
	public bool PrependStart { get; }
	public bool AppendEnd { get; }
	public bool IsSteppedLine { get; }
	public List<LineData> Data { get; }
	public Func<LineDataSet, LineData, List<MarkupString>> ToHighlighterValue { get; set; }
}
