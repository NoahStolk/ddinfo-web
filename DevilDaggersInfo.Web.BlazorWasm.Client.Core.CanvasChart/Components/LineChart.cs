using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.JsRuntime;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Components;

public partial class LineChart
{
	private Canvas2DContext _context = null!;
	private BECanvasComponent _canvasReference = null!;
	private LineChartHighlighter _highlighter = null!;

	private int _canvasWidth;
	private int _canvasHeight;

	private double _canvasMouseX;
	private double _canvasMouseY;

	private double ChartMouseX => _canvasMouseX - Options.ChartMarginXInPx;
	private double ChartMouseY => _canvasMouseY - Options.ChartMarginYInPx;

	private double ChartWidth => _canvasWidth - Options.ChartMarginXInPx * 2;
	private double ChartHeight => _canvasHeight - Options.ChartMarginYInPx * 2;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	[Parameter, EditorRequired] public string UniqueName { get; set; } = null!;
	[Parameter, EditorRequired] public List<LineDataSet> DataSets { get; set; } = null!;
	[Parameter, EditorRequired] public DataOptions DataOptions { get; set; } = null!;
	[Parameter] public LineChartOptions Options { get; set; } = new();
	[Parameter] public List<MarkupString> HighlighterValues { get; set; } = new();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await JsRuntime.InvokeAsync<object>("registerChart", DotNetObjectReference.Create(this), UniqueName);
			await JsRuntime.InvokeAsync<object>("windowResize", DotNetObjectReference.Create(this));
		}

		_context = await _canvasReference.CreateCanvas2DAsync();

		await RenderAsync();
	}

	private async Task RenderAsync()
	{
		// Clear canvas.
		await _context.ClearRectAsync(0, 0, _canvasWidth, _canvasHeight);

		// Determine grid.
		List<double> xScales = CalculateScales(ChartWidth, DataOptions.MinX, DataOptions.MaxX, DataOptions.StepX, Options.GridOptions.MinimumColumnWidthInPx);
		List<double> yScales = CalculateScales(ChartHeight, DataOptions.MinY, DataOptions.MaxY, DataOptions.StepY, Options.GridOptions.MinimumRowHeightInPx);

		// Set backgrounds.
		await _context.SetFillStyleAsync(Options.CanvasBackgroundColor);
		await _context.FillRectAsync(0, 0, _canvasWidth, _canvasHeight);
		await _context.SetFillStyleAsync(Options.ChartBackgroundColor);
		await _context.FillRectAsync(Options.ChartMarginXInPx, Options.ChartMarginYInPx, ChartWidth, ChartHeight);

		// Render graphics.
		await RenderGridAsync();
		await RenderSideBarsAsync();
		foreach (LineDataSet dataSet in DataSets)
			await RenderDataLineAsync(dataSet);
		await RenderHighlighterLineAsync();

		async Task RenderGridAsync()
		{
			await _context.SetLineWidthAsync(Options.GridOptions.LineThickness);
			await _context.SetStrokeStyleAsync(Options.GridOptions.LineColor);

			await _context.BeginPathAsync();
			for (int i = 0; i < xScales.Count; i++)
			{
				double xScalePosition = LerpUtils.RevLerp(DataOptions.MinX, DataOptions.MaxX, xScales[i]) * ChartWidth;
				await _context.MoveToAsync(Options.ChartMarginXInPx + xScalePosition, Options.ChartMarginYInPx);
				await _context.LineToAsync(Options.ChartMarginXInPx + xScalePosition, _canvasHeight - Options.ChartMarginYInPx);
			}

			for (int i = 0; i < yScales.Count; i++)
			{
				double yScalePosition = LerpUtils.RevLerp(DataOptions.MinY, DataOptions.MaxY, yScales[i]) * ChartHeight;
				await _context.MoveToAsync(Options.ChartMarginXInPx, Options.ChartMarginYInPx + yScalePosition);
				await _context.LineToAsync(_canvasWidth - Options.ChartMarginXInPx, Options.ChartMarginYInPx + yScalePosition);
			}

			await _context.StrokeAsync();
		}

		async Task RenderSideBarsAsync()
		{
			const int paddingX = 4;
			const int paddingY = 16;

			await _context.SetStrokeStyleAsync(Options.ScaleXOptions.TextColor);
			await _context.SetFontAsync(Options.ScaleXOptions.Font);
			await _context.SetTextAlignAsync(TextAlign.Center);
			for (int i = 0; i < xScales.Count; i++)
			{
				double xScaleValue = xScales[i];
				double xScalePosition = LerpUtils.RevLerp(DataOptions.MinX, DataOptions.MaxX, xScaleValue) * ChartWidth;
				await _context.StrokeTextAsync(xScaleValue.ToString(Options.ScaleXOptions.NumberFormat), Options.ChartMarginXInPx + xScalePosition, Options.ChartMarginYInPx + ChartHeight + paddingY);
			}

			await _context.SetStrokeStyleAsync(Options.ScaleYOptions.TextColor);
			await _context.SetFontAsync(Options.ScaleYOptions.Font);
			await _context.SetTextAlignAsync(TextAlign.Right);
			for (int i = 0; i < yScales.Count; i++)
			{
				double yScaleValue = yScales[i];
				double yScalePosition = LerpUtils.RevLerp(DataOptions.MinY, DataOptions.MaxY, yScaleValue) * ChartHeight;
				await _context.StrokeTextAsync(yScaleValue.ToString(Options.ScaleYOptions.NumberFormat), Options.ChartMarginXInPx - paddingX, Options.ChartMarginYInPx + ChartHeight - yScalePosition);
			}
		}

		async Task RenderDataLineAsync(LineDataSet dataSet)
		{
			if (dataSet.Data.Count < 2)
				return;

			await _context.SetLineWidthAsync(1);
			await _context.SetStrokeStyleAsync(dataSet.Color);

			await _context.BeginPathAsync();
			for (int i = 0; i < dataSet.Data.Count; i++)
			{
				LineData data = dataSet.Data[i];
				double percX = LerpUtils.RevLerp(DataOptions.MinX, DataOptions.MaxX, data.X);
				double percY = LerpUtils.RevLerp(DataOptions.MinY, DataOptions.MaxY, data.Y);
				double x = Options.ChartMarginXInPx + percX * ChartWidth;
				double y = Options.ChartMarginYInPx + ChartHeight - percY * ChartHeight;

				if (i == dataSet.Data.Count - 1)
				{
					await _context.LineToAsync(x, y);
					if (dataSet.AppendEnd && percX != 1)
						await _context.LineToAsync(Options.ChartMarginXInPx + ChartWidth, y);
				}
				else
				{
					if (i == 0)
					{
						if (dataSet.PrependStart && percX != 0)
						{
							await _context.MoveToAsync(Options.ChartMarginXInPx, y);
							await _context.LineToAsync(x, y);
						}
						else
						{
							await _context.MoveToAsync(x, y);
						}
					}
					else
					{
						await _context.LineToAsync(x, y);
					}

					if (dataSet.IsSteppedLine)
					{
						double percNextX = LerpUtils.RevLerp(DataOptions.MinX, DataOptions.MaxX, dataSet.Data[i + 1].X);
						double nextX = Options.ChartMarginXInPx + percNextX * ChartWidth;

						await _context.LineToAsync(nextX, y);
					}
				}
			}

			await _context.StrokeAsync();
		}

		async Task RenderHighlighterLineAsync()
		{
			if (Options.HighlighterLineOptions == null)
				return;

			await _context.SetLineWidthAsync(Options.HighlighterLineOptions.LineThickness);
			await _context.SetStrokeStyleAsync(Options.HighlighterLineOptions.LineColor);
			await _context.BeginPathAsync();
			await _context.MoveToAsync(_canvasMouseX, Options.ChartMarginYInPx);
			await _context.LineToAsync(_canvasMouseX, _canvasHeight - Options.ChartMarginYInPx);
			await _context.StrokeAsync();
		}
	}

	private static List<double> CalculateScales(double chartSize, double min, double max, double? step, double minimumSizeInPx)
	{
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
			scales = Enumerable.Range(0, calculatedCount).Select(i => i * calculatedStep).Append(max).ToList();
		}
		else
		{
			scales = new List<double>();
			for (double i = min; i <= max; i += step.Value)
				scales.Add(i);
		}

		return scales;
	}

	[JSInvokable]
	public async ValueTask OnResize(int screenWidth, int screenHeight)
	{
		_canvasWidth = (int)(screenWidth / 2.5f);
		_canvasHeight = (int)(screenHeight / 2.5f);

		await RenderAsync();
	}

	[JSInvokable]
	public async ValueTask OnMouseMove(int mouseX, int mouseY)
	{
		BoundingClientRect canvasBoundingClientRect = await JsRuntime.InvokeAsync<BoundingClientRect>("getBoundingClientRect", _canvasReference.CanvasReference);

		_canvasMouseX = mouseX - canvasBoundingClientRect.Left;
		_canvasMouseY = mouseY - canvasBoundingClientRect.Top;

		UpdateHighlighter();
		StateHasChanged();

		await ValueTask.CompletedTask;
	}

	private void UpdateHighlighter()
	{
		if (_highlighter == null)
			return;

		_highlighter.Left = Math.Clamp(_canvasMouseX, Options.ChartMarginXInPx, Options.ChartMarginXInPx + ChartWidth);
		_highlighter.Top = Math.Clamp(_canvasMouseY, Options.ChartMarginYInPx, Options.ChartMarginYInPx + ChartHeight);

		HighlighterValues.Clear();
		for (int i = 0; i < DataSets.Count; i++)
		{
			LineDataSet dataSet = DataSets[i];
			LineData? highlightedData = dataSet.Data.OrderByDescending(ld => ld.X).FirstOrDefault(ld => ld.X < ChartMouseX / ChartWidth * (DataOptions.MaxX - DataOptions.MinX)); // TODO: Fix for charts that don't start with MinX 0.
			if (highlightedData == null)
				continue;

			HighlighterValues.AddRange(dataSet.ToHighlighterValue(dataSet, highlightedData));
		}
	}
}
