using DevilDaggersInfo.App.Scenes;
using DevilDaggersInfo.App.Scenes.Base;
using DevilDaggersInfo.App.Ui;
using DevilDaggersInfo.App.Ui.ReplayEditor.State;
using DevilDaggersInfo.App.Ui.SpawnsetEditor.State;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.OpenGL;

namespace DevilDaggersInfo.App;

public static class Scene
{
	private static readonly SpawnsetBinary _mainMenuSpawnset = SpawnsetBinary.CreateDefault();

	private static ArenaScene? _mainMenuScene;
	private static ArenaScene? _spawnsetEditorScene;
	private static ArenaScene? _replayArenaScene;

	public static ArenaScene SpawnsetEditorScene => _spawnsetEditorScene ?? throw new InvalidOperationException("Scenes are not initialized.");
	public static ArenaScene ReplayArenaScene => _replayArenaScene ?? throw new InvalidOperationException("Scenes are not initialized.");

	public static void Initialize()
	{
		_mainMenuScene = new(static () => _mainMenuSpawnset, true, false);
		_mainMenuScene.AddSkull4();

		_spawnsetEditorScene = new(static () => SpawnsetState.Spawnset, false, true);
		_replayArenaScene = new(static () => ReplayState.Replay.Header.Spawnset, false, false);
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
