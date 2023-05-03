using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace DevilDaggersInfo.App;

public static class Root
{
	private static readonly InvalidOperationException _exception = new("Global context is not initialized.");

	private static InternalResources? _internalResources;
	private static GameResources? _gameResources;
	private static GL? _gl;
	private static IInputContext? _inputContext;
	private static IWindow? _window;

	public static InternalResources InternalResources => _internalResources ?? throw _exception;
	public static GameResources GameResources => _gameResources ?? throw _exception;
	public static GL Gl => _gl ?? throw _exception;
	public static IInputContext InputContext => _inputContext ?? throw _exception;
	public static IWindow Window => _window ?? throw _exception;

	public static void Initialize(InternalResources internalResources, GameResources gameResources, GL gl, IInputContext inputContext, IWindow window)
	{
		_internalResources = internalResources;
		_gameResources = gameResources;
		_gl = gl;
		_inputContext = inputContext;
		_window = window;
	}
}
