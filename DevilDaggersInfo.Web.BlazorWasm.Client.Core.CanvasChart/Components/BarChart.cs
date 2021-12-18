using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.JsRuntime;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.BarChart;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Data;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Components;

public partial class BarChart
{
	private Canvas2DContext _context = null!;
	private BECanvasComponent _canvasReference = null!;
	//private LineChartHighlighter _highlighter = null!;

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
	[Parameter, EditorRequired] public BarDataSet DataSet { get; set; } = null!;
	[Parameter, EditorRequired] public BarChartDataOptions DataOptions { get; set; } = null!;
	[Parameter, EditorRequired] public IEnumerable<string> XScaleTexts { get; set; } = null!;
	[Parameter] public BarChartOptions Options { get; set; } = new();
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
		//_highlighter.Width = Options.HighlighterWidth;

		// Clear canvas.
		await _context.ClearRectAsync(0, 0, _canvasWidth, _canvasHeight);

		// Determine grid.
		List<double> xScales = Enumerable.Range(0, DataSet.Data.Count).Select(i => (double)i).ToList();
		List<double> yScales = ScaleUtils.CalculateScales(ChartHeight, DataOptions.MinY, DataOptions.MaxY, DataOptions.StepY, Options.GridOptions.MinimumRowHeightInPx);
		double fullBarWidth = 1 / (double)DataSet.Data.Count * ChartWidth;

		// Set backgrounds.
		await _context.SetFillStyleAsync(Options.CanvasBackgroundColor);
		await _context.FillRectAsync(0, 0, _canvasWidth, _canvasHeight);
		await _context.SetFillStyleAsync(Options.ChartBackgroundColor);
		await _context.FillRectAsync(Options.ChartMarginXInPx, Options.ChartMarginYInPx, ChartWidth, ChartHeight);

		// Render graphics.
		await RenderGridAsync();
		await RenderSideBarsAsync();
		await RenderBarsAsync();

		async Task RenderGridAsync()
		{
			await _context.SetLineWidthAsync(Options.GridOptions.LineThickness);
			await _context.SetStrokeStyleAsync(Options.GridOptions.LineColor);

			await _context.BeginPathAsync();
			for (int i = 0; i < xScales.Count; i++)
			{
				double xScalePosition = i / (double)xScales.Count * ChartWidth;
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

			int skippedXScaleTexts = fullBarWidth switch
			{
				< 3 => 5,
				< 6 => 4,
				< 10 => 3,
				< 20 => 2,
				_ => 1,
			};
			List<string> xScaleTexts = XScaleTexts.ToList();
			for (int i = 0; i < xScales.Count; i++)
			{
				if (i >= xScaleTexts.Count || i % skippedXScaleTexts != 0)
					continue;

				double xScaleValue = xScales[i];
				double xScalePosition = LerpUtils.RevLerp(0, DataSet.Data.Count, xScaleValue) * ChartWidth + 1 / (double)DataSet.Data.Count * ChartWidth * 0.5;
				await _context.SaveAsync();
				await _context.TranslateAsync(Options.ChartMarginXInPx + xScalePosition, Options.ChartMarginYInPx + ChartHeight + paddingY);
				await _context.RotateAsync((360 - 45) * (MathF.PI / 180));
				await _context.StrokeTextAsync(xScaleTexts[i], 0, 0);
				await _context.RestoreAsync();
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

		async Task RenderBarsAsync()
		{
			if (DataSet.Data.Count == 0)
				return;

			for (int i = 0; i < DataSet.Data.Count; i++)
			{
				BarData data = DataSet.Data[i];
				double percX = i / (double)DataSet.Data.Count;
				double percY = LerpUtils.RevLerp(DataOptions.MinY, DataOptions.MaxY, data.Y);
				double x = percX * ChartWidth;
				double y = ChartHeight - percY * ChartHeight;

				double padding = fullBarWidth * 0.05;

				await _context.SetFillStyleAsync(data.Color);
				await _context.FillRectAsync(x + Options.ChartMarginXInPx + padding, y + Options.ChartMarginYInPx, fullBarWidth - padding * 2, ChartHeight - y);
			}
		}
	}

	[JSInvokable]
	public async ValueTask OnResize(double wrapperWidth, double wrapperHeight)
	{
		_canvasWidth = (int)wrapperWidth;
		_canvasHeight = (int)wrapperHeight;

		await RenderAsync();
	}

	[JSInvokable]
	public async ValueTask OnMouseMove(int mouseX, int mouseY)
	{
		BoundingClientRect canvasBoundingClientRect = await JsRuntime.InvokeAsync<BoundingClientRect>("getBoundingClientRect", _canvasReference.CanvasReference);

		_canvasMouseX = mouseX - canvasBoundingClientRect.Left;
		_canvasMouseY = mouseY - canvasBoundingClientRect.Top;

		//UpdateHighlighter();

		await ValueTask.CompletedTask;
	}

	//private void UpdateHighlighter()
	//{
	//	if (_highlighter == null)
	//		return;

	//	_highlighter.Left = Math.Clamp(_canvasMouseX, Options.ChartMarginXInPx, Options.ChartMarginXInPx + ChartWidth - _highlighter.Width);
	//	_highlighter.Top = Math.Clamp(_canvasMouseY, Options.ChartMarginYInPx, Options.ChartMarginYInPx + ChartHeight);

	//	double xValue = ChartMouseX / ChartWidth * (DataOptions.MaxX - DataOptions.MinX);

	//	HighlighterValues.Clear();
	//	for (int i = 0; i < DataSets.Count; i++)
	//	{
	//		LineDataSet dataSet = DataSets[i];
	//		if (dataSet.Data.Count == 0)
	//			continue;

	//		LineData highlightedData = dataSet.Data.OrderByDescending(ld => ld.X).FirstOrDefault(ld => ld.X < xValue + DataOptions.MinX) ?? dataSet.Data[0];
	//		HighlighterValues.AddRange(dataSet.ToHighlighterValue(dataSet, highlightedData));
	//	}

	//	_highlighter.ChangeState();

	//	//if (Options.HighlighterLineOptions == null)
	//	//	return;

	//	//await _context.SetLineWidthAsync(Options.HighlighterLineOptions.LineThickness);
	//	//await _context.SetStrokeStyleAsync(Options.HighlighterLineOptions.LineColor);
	//	//await _context.BeginPathAsync();
	//	//await _context.MoveToAsync(_canvasMouseX, Options.ChartMarginYInPx);
	//	//await _context.LineToAsync(_canvasMouseX, _canvasHeight - Options.ChartMarginYInPx);
	//	//await _context.StrokeAsync();
	//}

	protected override bool ShouldRender() => _shouldRender;

	private void SetHighlighterVisibility(bool visible)
	{
		// Prevent re-rendering the entire canvas.
		_shouldRender = false;
		//if (_highlighter != null)
		//{
		//	_highlighter.IsVisible = visible;
		//	_highlighter.ChangeState();
		//}
	}
}
