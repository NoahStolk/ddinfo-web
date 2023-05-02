using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using ErrorCode = Silk.NET.GLFW.ErrorCode;
using Monitor = Silk.NET.GLFW.Monitor;

namespace DevilDaggersInfo.App.Engine;

public static class Graphics
{
	private static bool _windowIsActive = true;
	private static GL? _gl;

	public static GL Gl
	{
		get => _gl ?? throw new InvalidOperationException("OpenGL is not initialized.");
		set => _gl = value;
	}

	public static Action<bool>? OnChangeWindowIsActive { get; set; }
	public static Action<int, int>? OnChangeWindowSize { get; set; }

	public static unsafe WindowHandle* Window { get; private set; }

	public static WindowState InitialWindowState { get; private set; }
	public static WindowState CurrentWindowState { get; private set; }
	public static bool WindowIsActive
	{
		get => _windowIsActive;
		private set
		{
			_windowIsActive = value;
			OnChangeWindowIsActive?.Invoke(_windowIsActive);
		}
	}
}
