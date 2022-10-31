using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Silk.NET.OpenGL;
using Warp.Content;
using Warp.InterpolationStates;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.GameObjects;

public class RaceDagger
{
	private readonly uint _daggerVao;
	private readonly Vector3State _position;
	private readonly QuaternionState _rotation;

	public RaceDagger(Vector3 position)
	{
		_daggerVao = CreateVao(ContentManager.Content.DaggerMesh);
		_position = new(position);
		_rotation = new(Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * 0.5f));

		static unsafe uint CreateVao(Mesh mesh)
		{
			uint vao = Gl.GenVertexArray();
			Gl.BindVertexArray(vao);

			uint vbo = Gl.GenBuffer();
			Gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

			fixed (Vertex* v = &mesh.Vertices[0])
				Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(mesh.Vertices.Length * sizeof(Vertex)), v, BufferUsageARB.StaticDraw);

			Gl.EnableVertexAttribArray(0);
			Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)0);

			Gl.EnableVertexAttribArray(1);
			Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(3 * sizeof(float)));

			// TODO: We don't do anything with normals here.
			Gl.EnableVertexAttribArray(2);
			Gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(5 * sizeof(float)));

			Gl.BindVertexArray(0);

			Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);

			return vao;
		}
	}

	public void Update()
	{
		_position.PrepareUpdate();
		_rotation.PrepareUpdate();

		_position.Physics = _position.Start + new Vector3(0, 0.15f + MathF.Sin(Root.Game.Tt) * 0.15f, 0);
		_rotation.Physics = _rotation.Start * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, Root.Game.Tt);
	}

	public unsafe void Render()
	{
		_position.PrepareRender();
		_rotation.PrepareRender();

		ContentManager.Content.DaggerSilverTexture.Use();

		Matrix4x4 model = Matrix4x4.CreateScale(8) * Matrix4x4.CreateFromQuaternion(_rotation.Render) * Matrix4x4.CreateTranslation(_position.Render);
		Shader.SetMatrix4x4(MeshUniforms.Model, model);

		Gl.BindVertexArray(_daggerVao);
		fixed (uint* i = &ContentManager.Content.DaggerMesh.Indices[0])
			Gl.DrawElements(PrimitiveType.Triangles, (uint)ContentManager.Content.DaggerMesh.Indices.Length, DrawElementsType.UnsignedInt, i);
		Gl.BindVertexArray(0);
	}
}