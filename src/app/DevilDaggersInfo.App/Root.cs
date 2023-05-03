using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.App.Core.NativeInterface.Services.Windows;
using DevilDaggersInfo.App.Ui.Base.Platforms;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace DevilDaggersInfo.App;

public static class Root
{
	private static readonly InvalidOperationException _notInitializedException = new("Root component is not initialized.");
	private static readonly InvalidOperationException _alreadyInitializedException = new("Root component is already initialized.");

	private static InternalResources? _internalResources;
	private static GameResources? _gameResources;
	private static GL? _gl;
	private static IInputContext? _inputContext;
	private static IWindow? _window;

	/// <summary>
	/// Holds the internal resources, such as shaders and icons.
	/// </summary>
	public static InternalResources InternalResources
	{
		get => _internalResources ?? throw _notInitializedException;
		set
		{
			if (_internalResources != null)
				throw _alreadyInitializedException;

			_internalResources = value;
		}
	}

	/// <summary>
	/// Holds the game resources, such as the tile texture and dagger mesh.
	/// </summary>
	public static GameResources GameResources
	{
		get => _gameResources ?? throw _notInitializedException;
		set
		{
			if (_gameResources != null)
				throw _alreadyInitializedException;

			_gameResources = value;
		}
	}

	public static GL Gl
	{
		get => _gl ?? throw _notInitializedException;
		set
		{
			if (_gl != null)
				throw _alreadyInitializedException;

			_gl = value;
		}
	}

	public static IInputContext InputContext
	{
		get => _inputContext ?? throw _notInitializedException;
		set
		{
			if (_inputContext != null)
				throw _alreadyInitializedException;

			_inputContext = value;
		}
	}

	public static IWindow Window
	{
		get => _window ?? throw _notInitializedException;
		set
		{
			if (_window != null)
				throw _alreadyInitializedException;

			_window = value;
		}
	}

#if WINDOWS
	public static INativeFileSystemService NativeFileSystemService { get; } = new WindowsFileSystemService();
	public static IPlatformSpecificValues PlatformSpecificValues { get; } = new WindowsValues();
#elif LINUX
	public static INativeFileSystemService NativeFileSystemService { get; } = new LinuxFileSystemService();
	public static IPlatformSpecificValues PlatformSpecificValues { get; } = new LinuxValues();
#endif
}
