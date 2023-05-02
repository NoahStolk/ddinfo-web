using DevilDaggersInfo.App.Engine.Content;
using DevilDaggersInfo.App.Ui.Base;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes.Base.GameObjects;

public class Skull4
{
	private static uint _vaoMain;
	private static uint _vaoJaw;

	public static unsafe void Initialize()
	{
		_vaoMain = CreateVao(GlobalContext.Gl, ContentManager.Content.Skull4Mesh);
		_vaoJaw = CreateVao(GlobalContext.Gl, ContentManager.Content.Skull4JawMesh);

		static uint CreateVao(GL gl, MeshContent mesh)
		{
			uint vao = gl.GenVertexArray();
			gl.BindVertexArray(vao);

			uint vbo = gl.GenBuffer();
			gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

			fixed (Vertex* v = &mesh.Vertices[0])
				gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(mesh.Vertices.Length * sizeof(Vertex)), v, BufferUsageARB.StaticDraw);

			gl.EnableVertexAttribArray(0);
			gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)0);

			gl.EnableVertexAttribArray(1);
			gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(3 * sizeof(float)));

			// TODO: We don't do anything with normals here.
			gl.EnableVertexAttribArray(2);
			gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(5 * sizeof(float)));

			gl.BindVertexArray(0);
			gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
			gl.DeleteBuffer(vbo);

			return vao;
		}
	}

	public unsafe void Render()
	{
		GL gl = GlobalContext.Gl;
		Shader meshShader = GlobalContext.InternalResources.MeshShader;

		meshShader.SetUniform("model", Matrix4x4.CreateScale(1.5f) * Matrix4x4.CreateTranslation(new(0, 4f, 0)));

		GlobalContext.GameResources.Skull4Texture.Bind();

		gl.BindVertexArray(_vaoMain);
		fixed (uint* i = &ContentManager.Content.Skull4Mesh.Indices[0])
			gl.DrawElements(PrimitiveType.Triangles, (uint)ContentManager.Content.Skull4Mesh.Indices.Length, DrawElementsType.UnsignedInt, i);

		GlobalContext.GameResources.Skull4JawTexture.Bind();

		gl.BindVertexArray(_vaoJaw);
		fixed (uint* i = &ContentManager.Content.Skull4JawMesh.Indices[0])
			gl.DrawElements(PrimitiveType.Triangles, (uint)ContentManager.Content.Skull4JawMesh.Indices.Length, DrawElementsType.UnsignedInt, i);

		gl.BindVertexArray(0);
	}
}
