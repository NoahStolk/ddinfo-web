using DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

namespace DevilDaggersInfo.Razor.Core.Canvas;

// TODO: Complete implementation.
public class WebViewCanvas2d : WebViewCanvas
{
	public WebViewCanvas2d(string id, WebViewRuntimeWrapper wrapper)
		: base(id, wrapper)
	{
	}

	public async Task SetFillStyleAsync(string fillStyle)
		=> await InvokeAsync("c2d.setFillStyle", fillStyle);

	public async Task SetStrokeStyleAsync(string strokeStyle)
		=> await InvokeAsync("c2d.setStrokeStyle", strokeStyle);

	public async Task SetLineWidthAsync(float lineWidth)
		=> await InvokeAsync("c2d.setLineWidth", lineWidth);

	public async Task ClearRectAsync(float x, float y, float width, float height)
		=> await InvokeAsync("c2d.clearRect", x, y, width, height);

	public async Task BeginPathAsync()
		=> await InvokeAsync("c2d.beginPath");

	public async Task ClosePathAsync()
		=> await InvokeAsync("c2d.closePath");

	public async Task MoveToAsync(float x, float y)
		=> await InvokeAsync("c2d.moveTo", x, y);

	public async Task LineToAsync(float x, float y)
		=> await InvokeAsync("c2d.lineTo", x, y);

	public async Task ArcAsync(float x, float y, float radius, float startAngle, float endAngle, bool anticlockwise = false)
		=> await InvokeAsync("c2d.arc", x, y, radius, startAngle, endAngle, anticlockwise ? 1 : 0);

	public async Task CircleAsync(float x, float y, float radius)
		=> await ArcAsync(x, y, radius, 0.0f, MathF.PI * 2);

	public async Task FillAsync()
		=> await InvokeAsync("c2d.fill");

	public async Task StrokeAsync()
		=> await InvokeAsync("c2d.stroke");
}
