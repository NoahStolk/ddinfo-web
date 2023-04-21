using Warp.NET.Maths.Numerics;

namespace Warp.NET.Extensions;

public static class ViewportExtensions
{
	public static void Activate(this Viewport viewport)
	{
		Gl.Gl.Viewport(viewport.X, viewport.Y, (uint)viewport.Width, (uint)viewport.Height);
	}
}
