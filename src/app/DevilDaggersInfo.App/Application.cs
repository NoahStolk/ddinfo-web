using DevilDaggersInfo.App.Layouts;
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

		// AppDomain.CurrentDomain.UnhandledException += (_, args) => Root.Dependencies.Log.Fatal(args.ExceptionObject.ToString());
		//
		// UserSettings.Load();
		// UserCache.Load();

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

	private void OnWindowOnRender(double delta)
	{
		if (_controller == null || _gl == null)
			throw new InvalidOperationException("Window has not loaded.");

		_controller.Update((float)delta);

		_gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		MainLayout.Render();
		//MainLayout.Render3d();

		_controller.Render();
	}

	private void OnWindowOnClosing()
	{
		_controller?.Dispose();
		_inputContext?.Dispose();
		_gl?.Dispose();
	}
}
