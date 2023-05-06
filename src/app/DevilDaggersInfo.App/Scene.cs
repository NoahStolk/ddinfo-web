using DevilDaggersInfo.App.Scenes;
using DevilDaggersInfo.App.Scenes.Base;
using Silk.NET.OpenGL;

namespace DevilDaggersInfo.App;

public static class Scene
{
	private static MainMenuArenaScene? _arenaScene;
	private static EditorArenaScene? _spawnsetEditorScene;
	private static ReplayArenaScene? _replayArenaScene;

	public static SceneType SceneType { get; set; }

	public static void Initialize()
	{
		_arenaScene = new();
		_spawnsetEditorScene = new();
		_replayArenaScene = new();
	}

	private static IArenaScene? GetScene()
	{
		return SceneType switch
		{
			SceneType.MainMenu => _arenaScene,
			SceneType.SpawnsetEditor => _spawnsetEditorScene,
			SceneType.ReplayEditor => _replayArenaScene,
			_ => throw new($"Invalid scene type {SceneType}."),
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
