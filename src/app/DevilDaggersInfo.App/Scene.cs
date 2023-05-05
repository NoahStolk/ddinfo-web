using DevilDaggersInfo.App.Scenes;
using Silk.NET.OpenGL;

namespace DevilDaggersInfo.App;

public static class Scene
{
	private static MainMenuArenaScene? _arenaScene;
	private static EditorArenaScene? _spawnsetEditorScene;

	public static SceneType SceneType { get; set; }

	public static void Initialize()
	{
		_arenaScene = new();
		_spawnsetEditorScene = new();
	}

	public static void Update(float delta)
	{
		switch (SceneType)
		{
			case SceneType.MainMenu:
				_arenaScene?.Update(0, delta);
				break;
			case SceneType.SpawnsetEditor:
				_spawnsetEditorScene?.Update(0);
				break;
		}
	}

	public static void Render(GL gl)
	{
		gl.Enable(EnableCap.DepthTest);
		gl.Enable(EnableCap.Blend);
		gl.Enable(EnableCap.CullFace);
		gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		switch (SceneType)
		{
			case SceneType.MainMenu:
				_arenaScene?.Render();
				break;
			case SceneType.SpawnsetEditor:
				_spawnsetEditorScene?.Render(0);
				break;
		}
	}
}
