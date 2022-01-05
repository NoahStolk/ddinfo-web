using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.JsRuntime;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using UmCanvas;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Components;

public partial class LineChart
{
	private Canvas2d? _context;
	private object? _canvasReference;
	private LineChartHighlighter? _highlighter;

	private int _canvasWidth;
	private int _canvasHeight;

	private double _canvasMouseX;
	private double _canvasMouseY;

	private bool _shouldRender = true;

	private double ChartMouseX => _canvasMouseX - Options.ChartMarginXInPx;
	private double ChartMouseY => _canvasMouseY - Options.ChartMarginYInPx;

	private double ChartWidth => _canvasWidth - Options.ChartMarginXInPx * 2;
	private double ChartHeight => _canvasHeight - Options.ChartMarginYInPx * 2;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	[Parameter, EditorRequired] public string UniqueName { get; set; } = null!;
	[Parameter, EditorRequired] public List<LineDataSet> DataSets { get; set; } = null!;
	[Parameter, EditorRequired] public LineChartDataOptions DataOptions { get; set; } = null!;
	[Parameter] public LineChartOptions Options { get; set; } = new();
	[Parameter] public List<MarkupString> HighlighterValues { get; set; } = new();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await JsRuntime.InvokeAsync<object>("registerChart", DotNetObjectReference.Create(this), UniqueName);
			await JsRuntime.InvokeAsync<object>("windowResize", DotNetObjectReference.Create(this));
		}

		_context = new Canvas2d($"{UniqueName}-canvas");

		Render();
	}

	private void Render()
	{
		if (_context == null || _highlighter == null)
			return;

		_highlighter.Width = Options.HighlighterWidth;

		// Determine grid.
		List<double> xScales = ScaleUtils.CalculateScales(ChartWidth, DataOptions.MinX, DataOptions.MaxX, DataOptions.StepX, Options.GridOptions.MinimumColumnWidthInPx, DataOptions.AllowFractionalScales);
		List<double> yScales = ScaleUtils.CalculateScales(ChartHeight, DataOptions.MinY, DataOptions.MaxY, DataOptions.StepY, Options.GridOptions.MinimumRowHeightInPx, DataOptions.AllowFractionalScales);

		// Render graphics.
		_context.ClearRect(0, 0, _canvasWidth, _canvasHeight);
		RenderBackground();
		RenderGrid();
		RenderSideBars();
		foreach (LineDataSet dataSet in DataSets)
			RenderDataLine(dataSet);

		void RenderBackground()
		{
			_context.FillStyle = Options.CanvasBackgroundColor;
			_context.FillRect(0, 0, _canvasWidth, _canvasHeight);
			_context.FillStyle = Options.ChartBackgroundColor;
			_context.FillRect(Options.ChartMarginXInPx, Options.ChartMarginYInPx, ChartWidth, ChartHeight);

			if (Options.Backgrounds?.Count > 0)
			{
				double end = DataOptions.MinX;
				foreach (LineChartBackground background in Options.Backgrounds)
				{
					double endX = LerpUtils.RevLerp(DataOptions.MinX, DataOptions.MaxX, background.ChartEndXValue) * ChartWidth;
					if (endX < 0)
						continue;

					double startX = LerpUtils.RevLerp(DataOptions.MinX, DataOptions.MaxX, end) * ChartWidth;
					if (startX < 0)
						startX = 0;

					_context.FillStyle=background.Color;
					_context.FillRect(Options.ChartMarginXInPx + startX, Options.ChartMarginYInPx, endX - startX, ChartHeight);

					end = background.ChartEndXValue;
				}
			}
		}

		void RenderGrid()
		{
			_context.LineWidth = Options.GridOptions.LineThickness;
			_context.StrokeStyle = Options.GridOptions.LineColor;

			_context.BeginPath();
			for (int i = 0; i < xScales.Count; i++)
			{
				double xScalePosition = LerpUtils.RevLerp(DataOptions.MinX, DataOptions.MaxX, xScales[i]) * ChartWidth;
				_context.MoveTo(Options.ChartMarginXInPx + xScalePosition, Options.ChartMarginYInPx);
				_context.LineTo(Options.ChartMarginXInPx + xScalePosition, _canvasHeight - Options.ChartMarginYInPx);
			}

			for (int i = 0; i < yScales.Count; i++)
			{
				double yScalePosition = LerpUtils.RevLerp(DataOptions.MinY, DataOptions.MaxY, yScales[i]) * ChartHeight;
				_context.MoveTo(Options.ChartMarginXInPx, Options.ChartMarginYInPx + yScalePosition);
				_context.LineTo(_canvasWidth - Options.ChartMarginXInPx, Options.ChartMarginYInPx + yScalePosition);
			}

			_context.Stroke();
		}

		void RenderSideBars()
		{
			const int paddingX = 4;
			const int paddingY = 16;

			_context.StrokeStyle = Options.ScaleXOptions.TextColor;
			_context.Font = Options.ScaleXOptions.Font;
			_context.TextAlign = TextAlign.Center;
			for (int i = 0; i < xScales.Count; i++)
			{
				double xScaleValue = xScales[i];
				double xScalePosition = LerpUtils.RevLerp(DataOptions.MinX, DataOptions.MaxX, xScaleValue) * ChartWidth;
				_context.StrokeText(Options.DisplayXScaleAsDates ? new DateTime((long)xScaleValue).ToString(FormatUtils.DateFormat) : xScaleValue.ToString(Options.ScaleXOptions.NumberFormat), Options.ChartMarginXInPx + xScalePosition, Options.ChartMarginYInPx + ChartHeight + paddingY);
			}

			_context.StrokeStyle=Options.ScaleYOptions.TextColor;
			_context.Font = Options.ScaleYOptions.Font;
			_context.TextAlign = TextAlign.Right;
			for (int i = 0; i < yScales.Count; i++)
			{
				double yScaleValue = yScales[i];
				double yScalePosition = LerpUtils.RevLerp(DataOptions.MinY, DataOptions.MaxY, yScaleValue) * ChartHeight;
				_context.StrokeText(yScaleValue.ToString(Options.ScaleYOptions.NumberFormat), Options.ChartMarginXInPx - paddingX, Options.ChartMarginYInPx + ChartHeight - yScalePosition);
			}
		}

		void RenderDataLine(LineDataSet dataSet)
		{
			if (dataSet.Data.Count < 2)
				return;

			List<LinePosition> linePositions = new();
			for (int i = 0; i < dataSet.Data.Count; i++)
			{
				LineData data = dataSet.Data[i];
				double percX = LerpUtils.RevLerp(DataOptions.MinX, DataOptions.MaxX, data.X);
				double percY = LerpUtils.RevLerp(DataOptions.MinY, DataOptions.MaxY, data.Y);
				double x = Options.ChartMarginXInPx + percX * ChartWidth;
				double y = Options.ChartMarginYInPx + ChartHeight - percY * ChartHeight;

				if (i == dataSet.Data.Count - 1)
				{
					linePositions.Add(new(x, y));
					if (dataSet.AppendEnd && percX != 1)
						linePositions.Add(new(Options.ChartMarginXInPx + ChartWidth, y));
				}
				else
				{
					if (i == 0)
					{
						if (dataSet.PrependStart && percX != 0)
						{
							linePositions.Add(new(Options.ChartMarginXInPx, y));
							linePositions.Add(new(x, y));
						}
						else
						{
							linePositions.Add(new(x, y));
						}
					}
					else
					{
						linePositions.Add(new(x, y));
					}

					if (dataSet.IsSteppedLine)
					{
						double percNextX = LerpUtils.RevLerp(DataOptions.MinX, DataOptions.MaxX, dataSet.Data[i + 1].X);
						double nextX = Options.ChartMarginXInPx + percNextX * ChartWidth;
						linePositions.Add(new(nextX, y));
					}
				}
			}

			_context.LineWidth = 1;
			_context.StrokeStyle = dataSet.Color;

			_context.BeginPath();

			for (int i = 0; i < linePositions.Count; i++)
			{
				LinePosition linePosition = linePositions[i];

				// Improve performance by skipping unnecessary line calls.
				if (i > 0 && i < linePositions.Count - 1 && linePosition.Y == linePositions[i - 1].Y && linePosition.Y == linePositions[i + 1].Y)
					continue;

				if (i == 0)
					_context.MoveTo(linePosition.X, linePosition.Y);
				else
					_context.LineTo(linePosition.X, linePosition.Y);
			}

			_context.Stroke();
		}
	}

	[JSInvokable]
	public void OnResize(double wrapperWidth, double wrapperHeight)
	{
		_canvasWidth = (int)wrapperWidth;
		_canvasHeight = (int)wrapperHeight;

		Render();
	}

	[JSInvokable]
	public async ValueTask OnMouseMove(int mouseX, int mouseY)
	{
		BoundingClientRect canvasBoundingClientRect = await JsRuntime.InvokeAsync<BoundingClientRect>("getBoundingClientRect", _canvasReference);

		_canvasMouseX = mouseX - canvasBoundingClientRect.Left;
		_canvasMouseY = mouseY - canvasBoundingClientRect.Top;

		UpdateHighlighter();

		await ValueTask.CompletedTask;
	}

	private void UpdateHighlighter()
	{
		if (_highlighter == null)
			return;

		_highlighter.Left = Math.Clamp(_canvasMouseX, Options.ChartMarginXInPx, Options.ChartMarginXInPx + ChartWidth - _highlighter.Width);
		_highlighter.Top = Math.Clamp(_canvasMouseY, Options.ChartMarginYInPx, Options.ChartMarginYInPx + ChartHeight);

		double xValue = ChartMouseX / ChartWidth * (DataOptions.MaxX - DataOptions.MinX);

		HighlighterValues.Clear();
		for (int i = 0; i < DataSets.Count; i++)
		{
			LineDataSet dataSet = DataSets[i];
			if (dataSet.Data.Count == 0)
				continue;

			LineData highlightedData = dataSet.Data.OrderByDescending(ld => ld.X).FirstOrDefault(ld => ld.X < xValue + DataOptions.MinX) ?? dataSet.Data[0];
			HighlighterValues.AddRange(dataSet.ToHighlighterValue(dataSet, highlightedData));
		}

		_highlighter.ChangeState();

		//if (Options.HighlighterLineOptions == null)
		//	return;

		//await _context.SetLineWidthAsync(Options.HighlighterLineOptions.LineThickness);
		//await _context.SetStrokeStyleAsync(Options.HighlighterLineOptions.LineColor);
		//await _context.BeginPathAsync();
		//await _context.MoveToAsync(_canvasMouseX, Options.ChartMarginYInPx);
		//await _context.LineToAsync(_canvasMouseX, _canvasHeight - Options.ChartMarginYInPx);
		//await _context.StrokeAsync();
	}

	protected override bool ShouldRender() => _shouldRender;

	private void SetHighlighterVisibility(bool visible)
	{
		// Prevent re-rendering the entire canvas.
		_shouldRender = false;
		if (_highlighter != null)
		{
			_highlighter.IsVisible = visible;
			_highlighter.ChangeState();
		}
	}

	private readonly record struct LinePosition(double X, double Y);
}
