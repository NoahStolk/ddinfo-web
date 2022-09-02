using DevilDaggersInfo.Razor.Core.Canvas;
using DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

namespace DevilDaggersInfo.Razor.Core.CanvasArena;

public class WebViewCanvasArena : WebViewCanvas2d
{
	public WebViewCanvasArena(string id, WebViewRuntimeWrapper runtimeWrapper)
		: base(id, runtimeWrapper)
	{
	}

	/// <summary>
	/// Invokes the window.drawTiles JS function.
	/// Note that color integers are used for performance and that multi-dimensional arrays are not supported when invoking JavaScript.
	/// </summary>
	public async Task DrawTilesAsync(int[] colors, float tileSize)
		=> await InvokeAsync("window.drawTiles", colors, tileSize);
}
