using Silk.NET.OpenGL;
using Warp.NET;

namespace DevilDaggersInfo.App.Ui.Base.Utils;

public static class VertexArrayObjectUtils
{
	public static unsafe uint CreateFromVertices(float[] vertices)
	{
		uint vao = Graphics.Gl.GenVertexArray();
		Graphics.Gl.BindVertexArray(vao);

		uint vbo = Graphics.Gl.GenBuffer();
		Graphics.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

		fixed (float* v = &vertices[0])
			Graphics.Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)vertices.Length * sizeof(float), v, BufferUsageARB.StaticDraw);

		Graphics.Gl.EnableVertexAttribArray(0);
		Graphics.Gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), (void*)0);

		Graphics.Gl.BindVertexArray(0);

		Graphics.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);

		return vao;
	}
}
