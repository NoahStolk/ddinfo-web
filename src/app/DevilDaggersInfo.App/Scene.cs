using DevilDaggersInfo.App.Scenes;
using Silk.NET.OpenGL;

namespace DevilDaggersInfo.App;

public static class Scene
{
	private static MainMenuArenaScene? _arenaScene;

	public static void Initialize()
	{
		_arenaScene = new();
	}

	public static void Update(float delta)
	{
		_arenaScene?.Update(0, delta);
	}

	public static void Render(GL gl)
	{
		gl.Enable(EnableCap.DepthTest);
		gl.Enable(EnableCap.Blend);
		gl.Enable(EnableCap.CullFace);
		gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		_arenaScene?.Render();
	}
}
