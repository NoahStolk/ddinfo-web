using DevilDaggersInfo.App.Layouts;
using DevilDaggersInfo.App.Scenes;
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

	private MainMenuArenaScene? _arenaScene;

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

	public AppVersion AppVersion { get; } // TODO: Use to check for updates.

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
		InternalResources internalResources = InternalResources.Create(_gl);

		// LOAD SETTINGS
		UserSettings.Load();
		UserCache.Load();

		// INIT DD CONTENT
		ContentManager.Initialize();
		GameResources gameResources = GameResources.Create(_gl);

		// INIT CONTEXT
		Root.Initialize(internalResources, gameResources, _gl, _inputContext, _window);

		_arenaScene = new();

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
		_arenaScene?.Update(0, (float)delta);
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

		_arenaScene?.Render();

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
