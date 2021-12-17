namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;

public class LineChartOptions
{
	public string CanvasBackgroundColor { get; set; } = "#080808";
	public string ChartBackgroundColor { get; set; } = "#0b0b0b";

	public double ChartMarginXInPx { get; set; } = 40;
	public double ChartMarginYInPx { get; set; } = 40;

	public LineChartGridOptions GridOptions { get; set; } = new();

	public LineChartScaleOptions ScaleXOptions { get; set; } = new();
	public LineChartScaleOptions ScaleYOptions { get; set; } = new();

	public double HighlighterWidth { get; set; } = 256;
	public List<string> HighlighterKeys { get; set; } = new();

	public LineChartHighlighterLineOptions? HighlighterLineOptions { get; set; } = new();
}
