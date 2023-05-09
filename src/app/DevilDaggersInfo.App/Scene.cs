using DevilDaggersInfo.App.Scenes;
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

	public static FramebufferData SpawnsetEditorFramebufferData { get; } = new();
	public static FramebufferData ReplayEditorFramebufferData { get; } = new();

	public static void Initialize()
	{
		_mainMenuScene = new(static () => _mainMenuSpawnset, true, false);
		_mainMenuScene.AddSkull4();

		_spawnsetEditorScene = new(static () => SpawnsetState.Spawnset, false, true);
		_replayArenaScene = new(static () => ReplayState.Replay.Header.Spawnset, false, false);
	}

	private static ArenaScene? GetScene()
	{
		return UiRenderer.Layout switch
		{
			LayoutType.Main => _mainMenuScene,
			LayoutType.SpawnsetEditor => _spawnsetEditorScene,
			LayoutType.ReplayEditor => ReplayArenaScene,
			_ => null,
		};
	}

	public static void Update(float delta)
	{
		ArenaScene? activeScene = GetScene();
		activeScene?.Update(delta);
	}

	public static unsafe void Render(GL gl)
	{
		FramebufferData? framebufferData = UiRenderer.Layout switch
		{
			LayoutType.SpawnsetEditor => SpawnsetEditorFramebufferData,
			LayoutType.ReplayEditor => ReplayEditorFramebufferData,
			_ => null,
		};

		gl.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferData?.Framebuffer ?? 0);

		int framebufferWidth = framebufferData?.Width ?? Root.Window.Size.X;
		int framebufferHeight = framebufferData?.Height ?? Root.Window.Size.Y;

		// Keep track of the original viewport so we can restore it later.
		Span<int> originalViewport = stackalloc int[4];
		gl.GetInteger(GLEnum.Viewport, originalViewport);
		gl.Viewport(0, 0, (uint)framebufferWidth, (uint)framebufferHeight);

		gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		gl.Enable(EnableCap.DepthTest);
		gl.Enable(EnableCap.Blend);
		gl.Enable(EnableCap.CullFace);
		gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		ArenaScene? activeScene = GetScene();
		activeScene?.Render(framebufferWidth, framebufferHeight);

		gl.Viewport(originalViewport[0], originalViewport[1], (uint)originalViewport[2], (uint)originalViewport[3]);
		gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
	}
}
