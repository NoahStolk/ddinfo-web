using DevilDaggersInfo.Razor.Core.Canvas;
using DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

namespace DevilDaggersInfo.Razor.Core.CanvasArena;

public class WebViewCanvasArena : WebViewCanvas
{
	public WebViewCanvasArena(string id, WebViewRuntimeWrapper runtimeWrapper)
		: base(id, runtimeWrapper)
	{
	}

	/// <summary>
	/// Invokes the JS function to draw the arena in its entirety at once. This is done for performance reasons.
	/// Note that color integers are also used for performance and that multi-dimensional arrays are not supported when invoking JavaScript.
	/// </summary>
	public async Task DrawArenaAsync(int[] colors, float canvasSize, float shrinkRadius, bool renderRaceDagger, float daggerX, float daggerY)
		=> await InvokeAsync("window.drawArena", colors, canvasSize, shrinkRadius, renderRaceDagger, daggerX, daggerY);
}
