using DevilDaggersInfo.Razor.Core.Canvas;
using System.Runtime.InteropServices.JavaScript;

namespace DevilDaggersInfo.Razor.Core.CanvasArena;

public partial class WebAssemblyCanvasArena : WebAssemblyCanvas2d
{
	private const string _moduleName = nameof(WebAssemblyCanvasArena);

	public WebAssemblyCanvasArena(string id)
		: base(id)
	{
	}

	public void DrawArena(int[] colors, int canvasSize, float shrinkRadius, bool shouldRenderRaceDagger, float daggerX, float daggerY)
		=> DrawArena(Id, colors, canvasSize, shrinkRadius, shouldRenderRaceDagger, daggerX, daggerY);

	[JSImport("drawArena", _moduleName)]
	private static partial void DrawArena(string id, int[] colors, int canvasSize, float shrinkRadius, bool shouldRenderRaceDagger, float daggerX, float daggerY);
}
