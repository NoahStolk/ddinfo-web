using DevilDaggersInfo.Razor.Core.Canvas;
using DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

namespace DevilDaggersInfo.Razor.Core.CanvasArena;

public class CanvasArena : Canvas2d
{
	public CanvasArena(IJSRuntimeWrapper wrapper, string id)
		: base(wrapper, id)
	{
	}

	public void DrawTile(int x, int y, int r, int g, int b, float tileSize)
		=> Invoke("window.drawTile", x, y, r, g, b, tileSize);
}
