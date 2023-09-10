using DevilDaggersInfo.App.Engine.ImGui;
using DevilDaggersInfo.App.Ui.ReplayEditor;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;

namespace DevilDaggersInfo.App.AppWindows;

public class OverlayWindow
{
	private ImGuiController? _imGuiController;
	private GL? _gl;

	public OverlayWindow()
	{
		WindowInstance = Window.Create(WindowOptions.Default with
		{
			WindowBorder = WindowBorder.Hidden,
			VSync = true,
			TransparentFramebuffer = true,
			TopMost = true,
		});

		WindowInstance.Load += OnWindowOnLoad;
		WindowInstance.FramebufferResize += OnWindowOnFramebufferResize;
		WindowInstance.Render += OnWindowOnRender;
		WindowInstance.Closing += OnWindowOnClosing;
	}

	public IWindow WindowInstance { get; }

	private void OnWindowOnLoad()
	{
		_gl = WindowInstance.CreateOpenGL();
		_imGuiController = new(_gl, WindowInstance, WindowInstance.CreateInput());

		_gl.ClearColor(0.2f, 0, 0, 0.2f);
	}

	private void OnWindowOnFramebufferResize(Vector2D<int> size)
	{
		_gl?.Viewport(size);
	}

	private void OnWindowOnRender(double delta)
	{
		if (_imGuiController == null || _gl == null)
			return;

		Vector2 ddWindowPosition = Root.GameWindowService.GetWindowPosition();
		WindowInstance.Position = new((int)ddWindowPosition.X, (int)ddWindowPosition.Y);
		WindowInstance.Size = new(320, 256);

		float deltaF = (float)delta;
		_imGuiController.Update(deltaF);

		_gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		ReplayOverlayWindow.Render();

		_imGuiController.Render();
	}

	private void OnWindowOnClosing()
	{
		_imGuiController?.Dispose();
		_gl?.Dispose();
	}
}
