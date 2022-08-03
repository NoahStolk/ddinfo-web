using DevilDaggersInfo.Razor.Core.Canvas;
using DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

namespace DevilDaggersInfo.Razor.Core.CanvasArena;

public class WebAssemblyCanvasArena : WebAssemblyCanvas2d
{
	public WebAssemblyCanvasArena(string id, WebAssemblyRuntimeWrapper runtimeWrapper)
		: base(id, runtimeWrapper)
	{
	}

	public void DrawTile(int x, int y, int r, int g, int b, float tileSize)
		=> Invoke("window.drawTile", x, y, r, g, b, tileSize);
}
