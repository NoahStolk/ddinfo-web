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
		// TODO: Prevent this from being called multiple times.
		_vaoMain = CreateVao(ContentManager.Content.Skull4Mesh);
		_vaoJaw = CreateVao(ContentManager.Content.Skull4JawMesh);

		static uint CreateVao(MeshContent mesh)
		{
			uint vao = Root.Gl.GenVertexArray();
			Root.Gl.BindVertexArray(vao);

			uint vbo = Root.Gl.GenBuffer();
			Root.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

			fixed (Vertex* v = &mesh.Vertices[0])
				Root.Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(mesh.Vertices.Length * sizeof(Vertex)), v, BufferUsageARB.StaticDraw);

			Root.Gl.EnableVertexAttribArray(0);
			Root.Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)0);

			Root.Gl.EnableVertexAttribArray(1);
			Root.Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(3 * sizeof(float)));

			// TODO: We don't do anything with normals here.
			Root.Gl.EnableVertexAttribArray(2);
			Root.Gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(5 * sizeof(float)));

			Root.Gl.BindVertexArray(0);
			Root.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
			Root.Gl.DeleteBuffer(vbo);

			return vao;
		}
	}

	public unsafe void Render()
	{
		Root.InternalResources.MeshShader.SetUniform("model", Matrix4x4.CreateScale(1.5f) * Matrix4x4.CreateTranslation(new(0, 4f, 0)));

		Root.GameResources.Skull4Texture.Bind();

		Root.Gl.BindVertexArray(_vaoMain);
		fixed (uint* i = &ContentManager.Content.Skull4Mesh.Indices[0])
			Root.Gl.DrawElements(PrimitiveType.Triangles, (uint)ContentManager.Content.Skull4Mesh.Indices.Length, DrawElementsType.UnsignedInt, i);

		Root.GameResources.Skull4JawTexture.Bind();

		Root.Gl.BindVertexArray(_vaoJaw);
		fixed (uint* i = &ContentManager.Content.Skull4JawMesh.Indices[0])
			Root.Gl.DrawElements(PrimitiveType.Triangles, (uint)ContentManager.Content.Skull4JawMesh.Indices.Length, DrawElementsType.UnsignedInt, i);

		Root.Gl.BindVertexArray(0);
	}
}
