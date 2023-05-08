using DevilDaggersInfo.App.Scenes;
using DevilDaggersInfo.App.Scenes.Base;
using DevilDaggersInfo.App.Ui;
using Silk.NET.OpenGL;

namespace DevilDaggersInfo.App;

public static class Scene
{
	private static ArenaScene? _mainMenuScene;
	private static EditorArenaScene? _spawnsetEditorScene;
	private static ArenaScene? _replayArenaScene;

	public static ArenaScene ReplayArenaScene
	{
		get => _replayArenaScene ?? throw new InvalidOperationException("Scenes are not initialized.");
		private set => _replayArenaScene = value;
	}

	public static void Initialize()
	{
		_mainMenuScene = new(true);
		_mainMenuScene.AddSkull4();

		_spawnsetEditorScene = new();
		ReplayArenaScene = new(false);
	}

	private static IArenaScene? GetScene()
	{
		return UiRenderer.Layout switch
		{
			LayoutType.Main => _mainMenuScene,
			LayoutType.SpawnsetEditor => _spawnsetEditorScene,
			LayoutType.ReplayEditor or LayoutType.CustomLeaderboards => ReplayArenaScene,
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
