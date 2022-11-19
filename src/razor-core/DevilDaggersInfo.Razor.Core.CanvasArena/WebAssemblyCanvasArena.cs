using DevilDaggersInfo.Razor.Core.Canvas;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Razor.Core.CanvasArena;

public class WebAssemblyCanvasArena : WebAssemblyCanvas2d
{
	public WebAssemblyCanvasArena(string id, IJSUnmarshalledRuntime runtime)
		: base(id, runtime)
	{
	}

	public void DrawTile(int x, int y, int r, int g, int b, float tileSize)
		=> Invoke("window.drawTile", x, y, r, g, b, tileSize);
}
