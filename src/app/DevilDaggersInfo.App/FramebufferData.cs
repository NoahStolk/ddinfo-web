using Silk.NET.OpenGL;

namespace DevilDaggersInfo.App;

public unsafe class FramebufferData
{
	private readonly float[] _screenVertices =
	{
		-1.0f, 1.0f, 0.0f, 1.0f,
		-1.0f, -1.0f, 0.0f, 0.0f,
		1.0f, -1.0f, 1.0f, 0.0f,

		-1.0f, 1.0f, 0.0f, 1.0f,
		1.0f, -1.0f, 1.0f, 0.0f,
		1.0f, 1.0f, 1.0f, 1.0f,
	};

	public uint TextureHandle { get; private set; }
	public uint Framebuffer { get; private set; }
	public int Width { get; private set; }
	public int Height { get; private set; }

	public void ResizeIfNecessary(int width, int height)
	{
		if (width == Width && height == Height)
			return;

		Width = width;
		Height = height;

		// Delete previous data.
		if (Framebuffer != 0)
			Root.Gl.DeleteFramebuffer(Framebuffer);

		if (TextureHandle != 0)
			Root.Gl.DeleteTexture(TextureHandle);

		// Create new data.
		uint screenVao = Root.Gl.GenVertexArray();
		uint screenVbo = Root.Gl.GenBuffer();
		Root.Gl.BindVertexArray(screenVao);
		Root.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, screenVbo);
		fixed (float* v = &_screenVertices[0])
			Root.Gl.BufferData(BufferTargetARB.ArrayBuffer, (uint)(sizeof(float) * _screenVertices.Length), v, BufferUsageARB.StaticDraw);
		Root.Gl.EnableVertexAttribArray(0);
		Root.Gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), (void*)0);
		Root.Gl.EnableVertexAttribArray(1);
		Root.Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), (void*)(2 * sizeof(float)));

		Framebuffer = Root.Gl.GenFramebuffer();
		Root.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, Framebuffer);

		TextureHandle = Root.Gl.GenTexture();
		Root.Gl.BindTexture(TextureTarget.Texture2D, TextureHandle);
		Root.Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb, (uint)Width, (uint)Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, null);
		Root.Gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
		Root.Gl.TexParameterI(TextureTarget.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Linear);
		Root.Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, TextureHandle, 0);

		uint rbo = Root.Gl.GenRenderbuffer();
		Root.Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);

		// Use a single renderbuffer object for both a depth AND stencil buffer.
		Root.Gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, (uint)Width, (uint)Height);
		Root.Gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);

		if (Root.Gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != GLEnum.FramebufferComplete)
			throw new("Framebuffer is not complete."); // TODO: Log warning instead.

		// Unbind.
		Root.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		Root.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
		Root.Gl.BindVertexArray(0);

		// Delete.
		Root.Gl.DeleteVertexArray(screenVao);
		Root.Gl.DeleteBuffer(screenVbo);
	}
}
