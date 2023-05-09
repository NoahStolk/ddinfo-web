using DevilDaggersInfo.App.Networking;
using DevilDaggersInfo.App.Networking.TaskHandlers;
using DevilDaggersInfo.App.Ui;
using DevilDaggersInfo.App.Ui.Config;
using DevilDaggersInfo.App.Ui.CustomLeaderboards;
using DevilDaggersInfo.App.User.Cache;
using DevilDaggersInfo.App.User.Settings;
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
		_window = Window.Create(WindowOptions.Default);

		Vector2D<int> windowSize = new(1366, 768);
		Vector2D<int> monitorSize = Silk.NET.Windowing.Monitor.GetMainMonitor(_window).Bounds.Size;
		_window.Size = windowSize;
		_window.Position = monitorSize / 2 - windowSize / 2;
		_window.Title = $"ddinfo tools {VersionUtils.EntryAssemblyVersion}";

		_window.Load += OnWindowOnLoad;
		_window.FramebufferResize += OnWindowOnFramebufferResize;
		_window.Render += OnWindowOnRender;
		_window.Closing += OnWindowOnClosing;

		if (!AppVersion.TryParse(VersionUtils.EntryAssemblyVersion, out AppVersion? appVersion))
			throw new InvalidOperationException("The current version number is invalid.");

		AppVersion = appVersion;
	}

	public static PerSecondCounter RenderCounter { get; } = new();
	public static float LastRenderDelta { get; private set; }

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
		_imGuiController = new(_gl, _window, _inputContext);

		_gl.ClearColor(0, 0, 0, 1);

		ConfigureImGui();

		UserSettings.Load();
		UserCache.Load();

		Root.InternalResources = InternalResources.Create(_gl);
		Root.Gl = _gl;
		Root.Mouse = _inputContext.Mice.Count == 0 ? null : _inputContext.Mice[0];
		Root.Keyboard = _inputContext.Keyboards.Count == 0 ? null : _inputContext.Keyboards[0];
		Root.Window = _window;
		Root.Application = this;

		SurvivalFileWatcher.Initialize();

		ConfigLayout.ValidateInstallation();

		// AppDomain.CurrentDomain.UnhandledException += (_, args) => Root.Dependencies.Log.Fatal(args.ExceptionObject.ToString());

		AsyncHandler.Run(
			static av =>
			{
				Modals.ShowUpdate = av != null;
				Modals.AvailableVersion = av;
			},
			() => FetchLatestVersion.HandleAsync(AppVersion, Root.PlatformSpecificValues.BuildType));
	}

	private static void ConfigureImGui()
	{
		ImGuiStylePtr style = ImGui.GetStyle();
		style.ScrollbarSize = 16;
		style.ScrollbarRounding = 0;

		ImGuiIOPtr io = ImGui.GetIO();
		io.WantSaveIniSettings = false;

		// This is mainly done for the arena editor, so the window is not moved when editing the arena.
		// TODO: I think we can also work around this by putting the arena inside a widget.
		io.ConfigWindowsMoveFromTitleBarOnly = true;
	}

	private void OnWindowOnFramebufferResize(Vector2D<int> s)
	{
		if (_gl == null)
			throw new InvalidOperationException("Window has not loaded.");

		_gl.Viewport(s);
	}

	private void OnWindowOnRender(double delta)
	{
		if (_imGuiController == null || _gl == null)
			throw new InvalidOperationException("Window has not loaded.");

		float deltaF = (float)delta;

		RenderCounter.Increment();
		LastRenderDelta = deltaF;

		UiRenderer.Update(deltaF);
		Scene.Update(deltaF);

		_imGuiController.Update(deltaF);

		_gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		UiRenderer.Render();
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
