using DevilDaggersInfo.App.Engine.Content;
using DevilDaggersInfo.App.Engine.InterpolationStates;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes.Base.GameObjects;

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
		GL gl = GlobalContext.Gl;
		Shader meshShader = GlobalContext.InternalResources.MeshShader;

		RotationState.PrepareRender();
		PositionState.PrepareRender();

		meshShader.SetUniform("model", Matrix4x4.CreateScale(4) * Matrix4x4.CreateFromQuaternion(RotationState.Render) * Matrix4x4.CreateTranslation(PositionState.Render));

		gl.BindVertexArray(_vao);
		fixed (uint* i = &_mesh.Indices[0])
			gl.DrawElements(PrimitiveType.Triangles, (uint)_mesh.Indices.Length, DrawElementsType.UnsignedInt, i);
		gl.BindVertexArray(0);
	}
}
