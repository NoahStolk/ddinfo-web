using DevilDaggersInfo.App.Scenes.Base.GameObjects;
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

	private ImGuiController? _imGuiController;
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
		_imGuiController = new(_gl, _window, _inputContext);

		_gl.ClearColor(0, 0, 0, 1);

		ImGuiStylePtr style = ImGui.GetStyle();
		style.ScrollbarSize = 32;
		style.ScrollbarRounding = 0;

		// Load internal resources, such as shaders and icons.
		InternalResources internalResources = InternalResources.Create(_gl);

		// Load settings.
		UserSettings.Load();
		UserCache.Load();

		// Load the DD content manager (based on the user settings) and initialize the DD resources.
		ContentManager.Initialize();
		GameResources gameResources = GameResources.Create(_gl);

		// Set up Root.
		Root.Initialize(internalResources, gameResources, _gl, _inputContext, _window);

		// Initialize 3D rendering.
		Skull4.Initialize();
		Tile.Initialize();

		// Initialize 3D scene.
		Scene.Initialize();

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

	private static void OnWindowOnUpdate(double delta)
	{
		Scene.Update((float)delta);
	}

	private void OnWindowOnRender(double delta)
	{
		if (_imGuiController == null || _gl == null)
			throw new InvalidOperationException("Window has not loaded.");

		_imGuiController.Update((float)delta);

		_gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		UiRenderer.RenderUi();
		Scene.Render(_gl);

		_imGuiController.Render();

		if (UiRenderer.WindowShouldClose)
			_window.Close();
	}

	private void OnWindowOnClosing()
	{
		_imGuiController?.Dispose();
		_inputContext?.Dispose();
		_gl?.Dispose();
	}
}
