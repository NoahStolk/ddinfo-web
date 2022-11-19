using DevilDaggersInfo.Razor.Core.Canvas;
using System.Runtime.InteropServices.JavaScript;

namespace DevilDaggersInfo.Razor.Core.CanvasArena;

public partial class WebAssemblyCanvasArena : WebAssemblyCanvas2d
{
	public WebAssemblyCanvasArena(string id)
		: base(id)
	{
	}

	public void DrawTile(int x, int y, int r, int g, int b, float tileSize)
		=> DrawTile(Id, x, y, r, g, b, tileSize);

	[JSImport("drawTile", ModuleName)]
	private static partial void DrawTile(string id, int x, int y, int r, int g, int b, float tileSize);
}
