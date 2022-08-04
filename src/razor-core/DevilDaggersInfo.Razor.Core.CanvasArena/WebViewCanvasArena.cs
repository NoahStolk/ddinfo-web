using DevilDaggersInfo.Razor.Core.Canvas;
using DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

namespace DevilDaggersInfo.Razor.Core.CanvasArena;

public class WebViewCanvasArena : WebViewCanvas2d
{
	public WebViewCanvasArena(string id, WebViewRuntimeWrapper runtimeWrapper)
		: base(id, runtimeWrapper)
	{
	}

	public async Task DrawTileAsync(int x, int y, int r, int g, int b, float tileSize)
		=> await InvokeAsync("window.drawTile", x, y, r, g, b, tileSize);
}
