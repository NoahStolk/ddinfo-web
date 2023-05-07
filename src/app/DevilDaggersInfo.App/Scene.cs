using DevilDaggersInfo.App.Scenes;
using DevilDaggersInfo.App.Scenes.Base;
using DevilDaggersInfo.App.Ui;
using Silk.NET.OpenGL;

namespace DevilDaggersInfo.App;

public static class Scene
{
	private static MainMenuArenaScene? _arenaScene;
	private static EditorArenaScene? _spawnsetEditorScene;
	private static ReplayArenaScene? _replayArenaScene;

	public static ReplayArenaScene? ReplayArenaScene => _replayArenaScene;

	public static void Initialize()
	{
		_arenaScene = new();
		_spawnsetEditorScene = new();
		_replayArenaScene = new();
	}

	private static IArenaScene? GetScene()
	{
		return UiRenderer.Layout switch
		{
			LayoutType.Main => _arenaScene,
			LayoutType.SpawnsetEditor => _spawnsetEditorScene,
			LayoutType.ReplayEditor or LayoutType.CustomLeaderboards => _replayArenaScene,
			_ => null,
		};
	}

	public static void Update(float delta)
	{
		IArenaScene? activeScene = GetScene();
		activeScene?.Update(delta);
	}

	public static void Render(GL gl)
	{
		gl.Enable(EnableCap.DepthTest);
		gl.Enable(EnableCap.Blend);
		gl.Enable(EnableCap.CullFace);
		gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		IArenaScene? activeScene = GetScene();
		activeScene?.Render();
	}
}
