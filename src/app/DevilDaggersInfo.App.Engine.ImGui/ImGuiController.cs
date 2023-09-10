using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace DevilDaggersInfo.App.Engine.ImGui;

public sealed class ImGuiController : IDisposable
{
	private static readonly Key[] _keyEnumArr = (Key[])Enum.GetValues(typeof(Key));

	private GL _gl = null!;
	private IView _view = null!;
	private IInputContext _input = null!;
	private bool _frameBegun;
	private readonly List<char> _pressedChars = new();
	private IKeyboard _keyboard = null!;

	private int _attribLocationTex;
	private int _attribLocationProjMtx;
	private int _attribLocationVtxPos;
	private int _attribLocationVtxUv;
	private int _attribLocationVtxColor;
	private uint _vboHandle;
	private uint _elementsHandle;
	private uint _vertexArrayObject;

	private Texture _fontTexture = null!;
	private Shader _shader = null!;

	private int _windowWidth;
	private int _windowHeight;

	private IntPtr _context;

	/// <summary>
	/// Constructs a new ImGuiController with font configuration.
	/// </summary>
	public ImGuiController(GL gl, IView view, IInputContext input, ImGuiFontConfig imGuiFontConfig)
		: this(gl, view, input, imGuiFontConfig, null)
	{
	}

	/// <summary>
	/// Constructs a new ImGuiController with an onConfigureIO Action.
	/// </summary>
	public ImGuiController(GL gl, IView view, IInputContext input, Action onConfigureIo)
		: this(gl, view, input, null, onConfigureIo)
	{
	}

	/// <summary>
	/// Constructs a new ImGuiController with font configuration and onConfigure Action.
	/// </summary>
	public ImGuiController(GL gl, IView view, IInputContext input, ImGuiFontConfig? imGuiFontConfig = null, Action? onConfigureIo = null)
	{
		Init(gl, view, input);

		ImGuiIOPtr io = ImGuiNET.ImGui.GetIO();
		if (imGuiFontConfig is not null)
		{
			IntPtr glyphRange = imGuiFontConfig.Value.GetGlyphRange?.Invoke(io) ?? default(IntPtr);

			io.Fonts.AddFontFromFileTTF(imGuiFontConfig.Value.FontPath, imGuiFontConfig.Value.FontSize, null, glyphRange);
		}

		onConfigureIo?.Invoke();

		io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

		CreateDeviceResources();
		SetKeyMappings();

		SetPerFrameImGuiData(1f / 60f);

		BeginFrame();
	}

	private void Init(GL gl, IView view, IInputContext input)
	{
		_gl = gl;
		_view = view;
		_input = input;
		_windowWidth = view.Size.X;
		_windowHeight = view.Size.Y;

		_context = ImGuiNET.ImGui.CreateContext();
		ImGuiNET.ImGui.SetCurrentContext(_context);
		ImGuiNET.ImGui.StyleColorsDark();
	}

	private void CreateDeviceResources()
	{
		// Backup GL state
		_gl.GetInteger(GLEnum.TextureBinding2D, out int lastTexture);
		_gl.GetInteger(GLEnum.ArrayBufferBinding, out int lastArrayBuffer);
		_gl.GetInteger(GLEnum.VertexArrayBinding, out int lastVertexArray);

		const string vertexSource =
			"""
			#version 330
			layout (location = 0) in vec2 Position;
			layout (location = 1) in vec2 UV;
			layout (location = 2) in vec4 Color;
			uniform mat4 ProjMtx;
			out vec2 Frag_UV;
			out vec4 Frag_Color;
			void main()
			{
				Frag_UV = UV;
				Frag_Color = Color;
				gl_Position = ProjMtx * vec4(Position.xy,0,1);
			}
			""";

		const string fragmentSource =
			"""
			#version 330
			in vec2 Frag_UV;
			in vec4 Frag_Color;
			uniform sampler2D Texture;
			layout (location = 0) out vec4 Out_Color;
			void main()
			{
				Out_Color = Frag_Color * texture(Texture, Frag_UV.st);
			}
			""";

		_shader = new Shader(_gl, vertexSource, fragmentSource);

		_attribLocationTex = _shader.GetUniformLocation("Texture");
		_attribLocationProjMtx = _shader.GetUniformLocation("ProjMtx");
		_attribLocationVtxPos = _shader.GetAttribLocation("Position");
		_attribLocationVtxUv = _shader.GetAttribLocation("UV");
		_attribLocationVtxColor = _shader.GetAttribLocation("Color");

		_vboHandle = _gl.GenBuffer();
		_elementsHandle = _gl.GenBuffer();

		RecreateFontDeviceTexture();

		// Restore modified GL state
		_gl.BindTexture(GLEnum.Texture2D, (uint)lastTexture);
		_gl.BindBuffer(GLEnum.ArrayBuffer, (uint)lastArrayBuffer);

		_gl.BindVertexArray((uint)lastVertexArray);
	}

	/// <summary>
	/// Creates the texture used to render text.
	/// </summary>
	private void RecreateFontDeviceTexture()
	{
		// Build texture atlas
		ImGuiIOPtr io = ImGuiNET.ImGui.GetIO();

		// Load as RGBA 32-bit (75% of the memory is wasted, but default font is so small) because it is more likely to be compatible with user's existing shaders. If your ImTextureId represent a higher-level concept than just a GL texture id, consider calling GetTexDataAsAlpha8() instead to save on GPU memory.
		io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int _);

		// Upload texture to graphics system
		_gl.GetInteger(GLEnum.TextureBinding2D, out int lastTexture);

		_fontTexture = new Texture(_gl, width, height, pixels);
		_fontTexture.Bind();
		_fontTexture.SetMagFilter(TextureMagFilter.Linear);
		_fontTexture.SetMinFilter(TextureMinFilter.Linear);

		// Store our identifier
		io.Fonts.SetTexID((IntPtr)_fontTexture.GlTexture);

		// Restore state
		_gl.BindTexture(GLEnum.Texture2D, (uint)lastTexture);
	}

	private void BeginFrame()
	{
		ImGuiNET.ImGui.NewFrame();
		_frameBegun = true;
		_keyboard = _input.Keyboards[0];
		_view.Resize += WindowResized;
		_keyboard.KeyChar += OnKeyChar;
	}

	private void OnKeyChar(IKeyboard arg1, char arg2)
	{
		_pressedChars.Add(arg2);
	}

	private void WindowResized(Vector2D<int> size)
	{
		_windowWidth = size.X;
		_windowHeight = size.Y;
	}

	/// <summary>
	/// Renders the ImGui draw list data.
	/// This method requires a <see cref="GraphicsDevice"/> because it may create new DeviceBuffers if the size of vertex
	/// or index data has increased beyond the capacity of the existing buffers.
	/// A <see cref="CommandList"/> is needed to submit drawing and resource update commands.
	/// </summary>
	public void Render()
	{
		if (_frameBegun)
		{
			IntPtr oldCtx = ImGuiNET.ImGui.GetCurrentContext();

			if (oldCtx != _context)
			{
				ImGuiNET.ImGui.SetCurrentContext(_context);
			}

			_frameBegun = false;
			ImGuiNET.ImGui.Render();
			RenderImDrawData(ImGuiNET.ImGui.GetDrawData());

			if (oldCtx != _context)
			{
				ImGuiNET.ImGui.SetCurrentContext(oldCtx);
			}
		}
	}

	/// <summary>
	/// Updates ImGui input and IO configuration state.
	/// </summary>
	public void Update(float deltaSeconds)
	{
		IntPtr oldCtx = ImGuiNET.ImGui.GetCurrentContext();

		if (oldCtx != _context)
		{
			ImGuiNET.ImGui.SetCurrentContext(_context);
		}

		if (_frameBegun)
		{
			ImGuiNET.ImGui.Render();
		}

		SetPerFrameImGuiData(deltaSeconds);
		UpdateImGuiInput();

		_frameBegun = true;
		ImGuiNET.ImGui.NewFrame();

		if (oldCtx != _context)
		{
			ImGuiNET.ImGui.SetCurrentContext(oldCtx);
		}
	}

	/// <summary>
	/// Sets per-frame data based on the associated window.
	/// This is called by Update(float).
	/// </summary>
	private void SetPerFrameImGuiData(float deltaSeconds)
	{
		ImGuiIOPtr io = ImGuiNET.ImGui.GetIO();
		io.DisplaySize = new Vector2(_windowWidth, _windowHeight);

		if (_windowWidth > 0 && _windowHeight > 0)
			io.DisplayFramebufferScale = new Vector2(_view.FramebufferSize.X / _windowWidth, _view.FramebufferSize.Y / _windowHeight);

		io.DeltaTime = deltaSeconds;
	}

	private void UpdateImGuiInput()
	{
		ImGuiIOPtr io = ImGuiNET.ImGui.GetIO();

		IMouse mouseState = _input.Mice[0];
		IKeyboard keyboardState = _input.Keyboards[0];

		io.MouseDown[0] = mouseState.IsButtonPressed(MouseButton.Left);
		io.MouseDown[1] = mouseState.IsButtonPressed(MouseButton.Right);
		io.MouseDown[2] = mouseState.IsButtonPressed(MouseButton.Middle);
		io.MousePos = mouseState.Position;

		ScrollWheel wheel = mouseState.ScrollWheels[0];
		io.MouseWheel = wheel.Y;
		io.MouseWheelH = wheel.X;

		for (int i = 0; i < _keyEnumArr.Length; i++)
		{
			Key key = _keyEnumArr[i];
			if (key == Key.Unknown)
				continue;

			io.KeysDown[(int)key] = keyboardState.IsKeyPressed(key);
		}

		for (int i = 0; i < _pressedChars.Count; i++)
		{
			char c = _pressedChars[i];
			io.AddInputCharacter(c);
		}

		_pressedChars.Clear();

		io.KeyCtrl = keyboardState.IsKeyPressed(Key.ControlLeft) || keyboardState.IsKeyPressed(Key.ControlRight);
		io.KeyAlt = keyboardState.IsKeyPressed(Key.AltLeft) || keyboardState.IsKeyPressed(Key.AltRight);
		io.KeyShift = keyboardState.IsKeyPressed(Key.ShiftLeft) || keyboardState.IsKeyPressed(Key.ShiftRight);
		io.KeySuper = keyboardState.IsKeyPressed(Key.SuperLeft) || keyboardState.IsKeyPressed(Key.SuperRight);
	}

	private static void SetKeyMappings()
	{
		ImGuiIOPtr io = ImGuiNET.ImGui.GetIO();
		io.KeyMap[(int)ImGuiKey.Tab] = (int)Key.Tab;
		io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Key.Left;
		io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Key.Right;
		io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Key.Up;
		io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Key.Down;
		io.KeyMap[(int)ImGuiKey.PageUp] = (int)Key.PageUp;
		io.KeyMap[(int)ImGuiKey.PageDown] = (int)Key.PageDown;
		io.KeyMap[(int)ImGuiKey.Home] = (int)Key.Home;
		io.KeyMap[(int)ImGuiKey.End] = (int)Key.End;
		io.KeyMap[(int)ImGuiKey.Delete] = (int)Key.Delete;
		io.KeyMap[(int)ImGuiKey.Backspace] = (int)Key.Backspace;
		io.KeyMap[(int)ImGuiKey.Enter] = (int)Key.Enter;
		io.KeyMap[(int)ImGuiKey.Escape] = (int)Key.Escape;
		io.KeyMap[(int)ImGuiKey.A] = (int)Key.A;
		io.KeyMap[(int)ImGuiKey.C] = (int)Key.C;
		io.KeyMap[(int)ImGuiKey.V] = (int)Key.V;
		io.KeyMap[(int)ImGuiKey.X] = (int)Key.X;
		io.KeyMap[(int)ImGuiKey.Y] = (int)Key.Y;
		io.KeyMap[(int)ImGuiKey.Z] = (int)Key.Z;
	}

	private unsafe void SetupRenderState(ImDrawDataPtr drawDataPtr)
	{
		// Set up render state: alpha-blending enabled, no face culling, no depth testing, scissor enabled, polygon fill.
		_gl.Enable(GLEnum.Blend);
		_gl.BlendEquation(GLEnum.FuncAdd);
		_gl.BlendFuncSeparate(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha, GLEnum.One, GLEnum.OneMinusSrcAlpha);
		_gl.Disable(GLEnum.CullFace);
		_gl.Disable(GLEnum.DepthTest);
		_gl.Disable(GLEnum.StencilTest);
		_gl.Enable(GLEnum.ScissorTest);
		_gl.Disable(GLEnum.PrimitiveRestart);
		_gl.PolygonMode(GLEnum.FrontAndBack, GLEnum.Fill);

		float l = drawDataPtr.DisplayPos.X;
		float r = drawDataPtr.DisplayPos.X + drawDataPtr.DisplaySize.X;
		float t = drawDataPtr.DisplayPos.Y;
		float b = drawDataPtr.DisplayPos.Y + drawDataPtr.DisplaySize.Y;

		Span<float> orthoProjection = stackalloc float[]
		{
			2.0f / (r - l), 0.0f, 0.0f, 0.0f,
			0.0f, 2.0f / (t - b), 0.0f, 0.0f,
			0.0f, 0.0f, -1.0f, 0.0f,
			(r + l) / (l - r), (t + b) / (b - t), 0.0f, 1.0f,
		};

		_shader.UseShader();
		_gl.Uniform1(_attribLocationTex, 0);
		_gl.UniformMatrix4(_attribLocationProjMtx, 1, false, orthoProjection);

		_gl.BindSampler(0, 0);

		// Setup desired GL state
		// Recreate the VAO every time (this is to easily allow multiple GL contexts to be rendered to. VAO are not shared among GL contexts)
		// The renderer would actually work without any VAO bound, but then our VertexAttrib calls would overwrite the default one currently bound.
		_vertexArrayObject = _gl.GenVertexArray();
		_gl.BindVertexArray(_vertexArrayObject);

		// Bind vertex/index buffers and setup attributes for ImDrawVert
		_gl.BindBuffer(GLEnum.ArrayBuffer, _vboHandle);
		_gl.BindBuffer(GLEnum.ElementArrayBuffer, _elementsHandle);
		_gl.EnableVertexAttribArray((uint)_attribLocationVtxPos);
		_gl.EnableVertexAttribArray((uint)_attribLocationVtxUv);
		_gl.EnableVertexAttribArray((uint)_attribLocationVtxColor);
		_gl.VertexAttribPointer((uint)_attribLocationVtxPos, 2, GLEnum.Float, false, (uint)sizeof(ImDrawVert), (void*)0);
		_gl.VertexAttribPointer((uint)_attribLocationVtxUv, 2, GLEnum.Float, false, (uint)sizeof(ImDrawVert), (void*)8);
		_gl.VertexAttribPointer((uint)_attribLocationVtxColor, 4, GLEnum.UnsignedByte, true, (uint)sizeof(ImDrawVert), (void*)16);
	}

	private unsafe void RenderImDrawData(ImDrawDataPtr drawDataPtr)
	{
		int framebufferWidth = (int)(drawDataPtr.DisplaySize.X * drawDataPtr.FramebufferScale.X);
		int framebufferHeight = (int)(drawDataPtr.DisplaySize.Y * drawDataPtr.FramebufferScale.Y);
		if (framebufferWidth <= 0 || framebufferHeight <= 0)
			return;

		// Backup GL state
		_gl.GetInteger(GLEnum.ActiveTexture, out int lastActiveTexture);
		_gl.ActiveTexture(GLEnum.Texture0);

		_gl.GetInteger(GLEnum.CurrentProgram, out int lastProgram);
		_gl.GetInteger(GLEnum.TextureBinding2D, out int lastTexture);

		_gl.GetInteger(GLEnum.SamplerBinding, out int lastSampler);

		_gl.GetInteger(GLEnum.ArrayBufferBinding, out int lastArrayBuffer);
		_gl.GetInteger(GLEnum.VertexArrayBinding, out int lastVertexArrayObject);

		Span<int> lastPolygonMode = stackalloc int[2];
		_gl.GetInteger(GLEnum.PolygonMode, lastPolygonMode);

		Span<int> lastScissorBox = stackalloc int[4];
		_gl.GetInteger(GLEnum.ScissorBox, lastScissorBox);

		_gl.GetInteger(GLEnum.BlendSrcRgb, out int lastBlendSrcRgb);
		_gl.GetInteger(GLEnum.BlendDstRgb, out int lastBlendDstRgb);

		_gl.GetInteger(GLEnum.BlendSrcAlpha, out int lastBlendSrcAlpha);
		_gl.GetInteger(GLEnum.BlendDstAlpha, out int lastBlendDstAlpha);

		_gl.GetInteger(GLEnum.BlendEquationRgb, out int lastBlendEquationRgb);
		_gl.GetInteger(GLEnum.BlendEquationAlpha, out int lastBlendEquationAlpha);

		bool lastEnableBlend = _gl.IsEnabled(GLEnum.Blend);
		bool lastEnableCullFace = _gl.IsEnabled(GLEnum.CullFace);
		bool lastEnableDepthTest = _gl.IsEnabled(GLEnum.DepthTest);
		bool lastEnableStencilTest = _gl.IsEnabled(GLEnum.StencilTest);
		bool lastEnableScissorTest = _gl.IsEnabled(GLEnum.ScissorTest);
		bool lastEnablePrimitiveRestart = _gl.IsEnabled(GLEnum.PrimitiveRestart);

		SetupRenderState(drawDataPtr);

		// Will project scissor/clipping rectangles into framebuffer space
		Vector2 clipOff = drawDataPtr.DisplayPos; // (0,0) unless using multi-viewports
		Vector2 clipScale = drawDataPtr.FramebufferScale; // (1,1) unless using retina display which are often (2,2)

		// Render command lists
		for (int n = 0; n < drawDataPtr.CmdListsCount; n++)
		{
			ImDrawListPtr cmdListPtr = drawDataPtr.CmdListsRange[n];

			// Upload vertex/index buffers
			_gl.BufferData(GLEnum.ArrayBuffer, (nuint)(cmdListPtr.VtxBuffer.Size * sizeof(ImDrawVert)), (void*)cmdListPtr.VtxBuffer.Data, GLEnum.StreamDraw);
			_gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(cmdListPtr.IdxBuffer.Size * sizeof(ushort)), (void*)cmdListPtr.IdxBuffer.Data, GLEnum.StreamDraw);

			for (int cmdI = 0; cmdI < cmdListPtr.CmdBuffer.Size; cmdI++)
			{
				ImDrawCmdPtr cmdPtr = cmdListPtr.CmdBuffer[cmdI];

				if (cmdPtr.UserCallback != IntPtr.Zero)
					throw new NotImplementedException();

				Vector4 clipRect;
				clipRect.X = (cmdPtr.ClipRect.X - clipOff.X) * clipScale.X;
				clipRect.Y = (cmdPtr.ClipRect.Y - clipOff.Y) * clipScale.Y;
				clipRect.Z = (cmdPtr.ClipRect.Z - clipOff.X) * clipScale.X;
				clipRect.W = (cmdPtr.ClipRect.W - clipOff.Y) * clipScale.Y;

				if (clipRect.X < framebufferWidth && clipRect.Y < framebufferHeight && clipRect.Z >= 0.0f && clipRect.W >= 0.0f)
				{
					// Apply scissor/clipping rectangle
					_gl.Scissor((int)clipRect.X, (int)(framebufferHeight - clipRect.W), (uint)(clipRect.Z - clipRect.X), (uint)(clipRect.W - clipRect.Y));

					// Bind texture, Draw
					_gl.BindTexture(GLEnum.Texture2D, (uint)cmdPtr.TextureId);

					_gl.DrawElementsBaseVertex(GLEnum.Triangles, cmdPtr.ElemCount, GLEnum.UnsignedShort, (void*)(cmdPtr.IdxOffset * sizeof(ushort)), (int)cmdPtr.VtxOffset);
				}
			}
		}

		// Destroy the temporary VAO
		_gl.DeleteVertexArray(_vertexArrayObject);
		_vertexArrayObject = 0;

		// Restore modified GL state
		_gl.UseProgram((uint)lastProgram);
		_gl.BindTexture(GLEnum.Texture2D, (uint)lastTexture);

		_gl.BindSampler(0, (uint)lastSampler);

		_gl.ActiveTexture((GLEnum)lastActiveTexture);

		_gl.BindVertexArray((uint)lastVertexArrayObject);

		_gl.BindBuffer(GLEnum.ArrayBuffer, (uint)lastArrayBuffer);
		_gl.BlendEquationSeparate((GLEnum)lastBlendEquationRgb, (GLEnum)lastBlendEquationAlpha);
		_gl.BlendFuncSeparate((GLEnum)lastBlendSrcRgb, (GLEnum)lastBlendDstRgb, (GLEnum)lastBlendSrcAlpha, (GLEnum)lastBlendDstAlpha);

		if (lastEnableBlend)
			_gl.Enable(GLEnum.Blend);
		else
			_gl.Disable(GLEnum.Blend);

		if (lastEnableCullFace)
			_gl.Enable(GLEnum.CullFace);
		else
			_gl.Disable(GLEnum.CullFace);

		if (lastEnableDepthTest)
			_gl.Enable(GLEnum.DepthTest);
		else
			_gl.Disable(GLEnum.DepthTest);

		if (lastEnableStencilTest)
			_gl.Enable(GLEnum.StencilTest);
		else
			_gl.Disable(GLEnum.StencilTest);

		if (lastEnableScissorTest)
			_gl.Enable(GLEnum.ScissorTest);
		else
			_gl.Disable(GLEnum.ScissorTest);

		if (lastEnablePrimitiveRestart)
			_gl.Enable(GLEnum.PrimitiveRestart);
		else
			_gl.Disable(GLEnum.PrimitiveRestart);

		_gl.PolygonMode(GLEnum.FrontAndBack, (GLEnum)lastPolygonMode[0]);

		_gl.Scissor(lastScissorBox[0], lastScissorBox[1], (uint)lastScissorBox[2], (uint)lastScissorBox[3]);
	}

	/// <summary>
	/// Frees all graphics resources used by the renderer.
	/// </summary>
	public void Dispose()
	{
		_view.Resize -= WindowResized;
		_keyboard.KeyChar -= OnKeyChar;

		_gl.DeleteBuffer(_vboHandle);
		_gl.DeleteBuffer(_elementsHandle);
		_gl.DeleteVertexArray(_vertexArrayObject);

		_fontTexture.Dispose();
		_shader.Dispose();

		ImGuiNET.ImGui.DestroyContext(_context);
	}
}
