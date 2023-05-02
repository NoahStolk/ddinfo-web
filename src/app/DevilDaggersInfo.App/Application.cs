using DevilDaggersInfo.App.Engine;
using DevilDaggersInfo.App.Engine.Content;
using DevilDaggersInfo.App.Layouts;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.User.Cache;
using DevilDaggersInfo.App.Ui.Base.User.Settings;
using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Core.Versioning;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace DevilDaggersInfo.App;

public class Application
{
	private readonly IWindow _window;

	private ImGuiController? _controller;
	private GL? _gl;
	private IInputContext? _inputContext;

	public Application()
	{
		const int monitorWidth = 3840; // TODO: Get from monitor.
		const int monitorHeight = 2160;
		const int windowWidth = 1024;
		const int windowHeight = 768;

		_window = Window.Create(WindowOptions.Default with
		{
			Position = new(monitorWidth / 2 - windowWidth / 2, monitorHeight / 2 - windowHeight / 2),
			Size = new(windowWidth, windowHeight),
		});
		_window.Load += OnWindowOnLoad;
		_window.FramebufferResize += OnWindowOnFramebufferResize;
		_window.Update += OnWindowOnUpdate;
		_window.Render += OnWindowOnRender;
		_window.Closing += OnWindowOnClosing;

		if (!AppVersion.TryParse(VersionUtils.EntryAssemblyVersion, out AppVersion? appVersion))
			throw new InvalidOperationException("The current version number is invalid.");

		AppVersion = appVersion;
	}

	public AppVersion AppVersion { get; }

	public void Run()
	{
		_window.Run();
	}

	public void Destroy()
	{
		_window.Dispose();
	}

	private void OnWindowOnLoad()
	{
		_gl = _window.CreateOpenGL();
		_inputContext = _window.CreateInput();
		_controller = new(_gl, _window, _inputContext);

		_gl.ClearColor(0, 0, 0, 1);

		ImGuiStylePtr style = ImGui.GetStyle();
		style.ScrollbarSize = 32;
		style.ScrollbarRounding = 0;

		// INIT DDINFO CONTENT
#if DEBUG
		const string? ddInfoToolsContentRootDirectory = @"..\..\..\..\..\app\DevilDaggersInfo.App.Ui.Base\Content";
#else
		const string? ddInfoToolsContentRootDirectory = null;
#endif
		DecompiledContentFile ddInfoToolsContent = Bootstrapper.GetDecompiledContent(ddInfoToolsContentRootDirectory, "ddinfo");

		ddInfoToolsContent.Shaders.TryGetValue("Mesh", out ShaderContent? meshShaderContent);
		if (meshShaderContent == null)
			throw new InvalidOperationException("Could not find mesh shader.");

		ddInfoToolsContent.Textures.TryGetValue("TileHitbox", out TextureContent? tileHitboxContent);
		if (tileHitboxContent == null)
			throw new InvalidOperationException("Could not find tile hitbox texture.");

		ddInfoToolsContent.Models.TryGetValue("TileHitbox", out ModelContent? tileHitboxModelContent);
		if (tileHitboxModelContent == null)
			throw new InvalidOperationException("Could not find tile hitbox model.");

		Shader meshShader = new(_gl, meshShaderContent.VertexCode, meshShaderContent.FragmentCode);
		Texture tileHitbox = new(_gl, tileHitboxContent.Pixels, (uint)tileHitboxContent.Width, (uint)tileHitboxContent.Height);
		InternalResources internalResources = new(meshShader, tileHitbox, tileHitboxModelContent);

		UserSettings.Load();
		UserCache.Load();

		// INIT DD CONTENT
		ContentManager.Initialize();

		Texture iconDaggerTexture = new(_gl, ContentManager.Content.IconDaggerTexture.Pixels, (uint)ContentManager.Content.IconDaggerTexture.Width, (uint)ContentManager.Content.IconDaggerTexture.Height);
		Texture daggerSilverTexture = new(_gl, ContentManager.Content.DaggerSilverTexture.Pixels, (uint)ContentManager.Content.DaggerSilverTexture.Width, (uint)ContentManager.Content.DaggerSilverTexture.Height);
		Texture skull4Texture = new(_gl, ContentManager.Content.Skull4Texture.Pixels, (uint)ContentManager.Content.Skull4Texture.Width, (uint)ContentManager.Content.Skull4Texture.Height);
		Texture skull4JawTexture = new(_gl, ContentManager.Content.Skull4JawTexture.Pixels, (uint)ContentManager.Content.Skull4JawTexture.Width, (uint)ContentManager.Content.Skull4JawTexture.Height);
		Texture tileTexture = new(_gl, ContentManager.Content.TileTexture.Pixels, (uint)ContentManager.Content.TileTexture.Width, (uint)ContentManager.Content.TileTexture.Height);
		Texture pillarTexture = new(_gl, ContentManager.Content.PillarTexture.Pixels, (uint)ContentManager.Content.PillarTexture.Width, (uint)ContentManager.Content.PillarTexture.Height);
		Texture postLut = new(_gl, ContentManager.Content.PostLut.Pixels, (uint)ContentManager.Content.PostLut.Width, (uint)ContentManager.Content.PostLut.Height);
		Texture hand4Texture = new(_gl, ContentManager.Content.Hand4Texture.Pixels, (uint)ContentManager.Content.Hand4Texture.Width, (uint)ContentManager.Content.Hand4Texture.Height);
		GameResources gameResources = new(iconDaggerTexture, daggerSilverTexture, skull4Texture, skull4JawTexture, tileTexture, pillarTexture, postLut, hand4Texture);

		// INIT CONTEXT
		GlobalContext.Initialize(internalResources, gameResources, _gl, _inputContext, _window);

		MainLayout.Initialize();

		// AppDomain.CurrentDomain.UnhandledException += (_, args) => Root.Dependencies.Log.Fatal(args.ExceptionObject.ToString());

		//AsyncHandler.Run(ShowUpdateAvailable, () => FetchLatestVersion.HandleAsync(Root.Game.AppVersion, Root.Dependencies.PlatformSpecificValues.BuildType));
		// private static void ShowUpdateAvailable(AppVersion? newAppVersion)
		// {
		// 	if (newAppVersion != null)
		// 		Root.Dependencies.NativeDialogService.ReportMessage("Update available", $"Version {newAppVersion} is available. Re-run the launcher to install it.");
		// }
	}

	private void OnWindowOnFramebufferResize(Vector2D<int> s)
	{
		if (_gl == null)
			throw new InvalidOperationException("Window has not loaded.");

		_gl.Viewport(s);
	}

	private void OnWindowOnUpdate(double delta)
	{
		MainLayout.Update((float)delta);
	}

	private void OnWindowOnRender(double delta)
	{
		if (_controller == null || _gl == null)
			throw new InvalidOperationException("Window has not loaded.");

		_controller.Update((float)delta);

		_gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		MainLayout.Render(out bool shouldClose);

		_gl.Enable(EnableCap.DepthTest);
		_gl.Enable(EnableCap.Blend);
		_gl.Enable(EnableCap.CullFace);
		_gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		MainLayout.Render3d();

		_controller.Render();

		if (shouldClose)
			_window.Close();
	}

	private void OnWindowOnClosing()
	{
		_controller?.Dispose();
		_inputContext?.Dispose();
		_gl?.Dispose();
	}
}
