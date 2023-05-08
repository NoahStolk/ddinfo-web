using DevilDaggersInfo.App.Engine.Content;
using DevilDaggersInfo.App.Engine.InterpolationStates;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes.GameObjects;

public class PlayerMovement
{
	private readonly uint _vao;
	private readonly MeshContent _mesh;

	public PlayerMovement(uint vao, MeshContent mesh, Quaternion rotation, Vector3 position)
	{
		_vao = vao;
		_mesh = mesh;

		RotationState = new(rotation);
		PositionState = new(position);
	}

	public QuaternionState RotationState { get; }

	public Vector3State PositionState { get; }

	public void PrepareUpdate()
	{
		RotationState.PrepareUpdate();
		PositionState.PrepareUpdate();
	}

	public unsafe void Render()
	{
		RotationState.PrepareRender();
		PositionState.PrepareRender();

		Root.InternalResources.MeshShader.SetUniform("model", Matrix4x4.CreateScale(4) * Matrix4x4.CreateFromQuaternion(RotationState.Render) * Matrix4x4.CreateTranslation(PositionState.Render));

		Root.Gl.BindVertexArray(_vao);
		fixed (uint* i = &_mesh.Indices[0])
			Root.Gl.DrawElements(PrimitiveType.Triangles, (uint)_mesh.Indices.Length, DrawElementsType.UnsignedInt, i);
		Root.Gl.BindVertexArray(0);
	}
}
