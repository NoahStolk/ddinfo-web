using DevilDaggersInfo.App.Engine.Content;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes.Base.GameObjects;

public class TileMeshObject
{
	private readonly uint _vao;
	private readonly MeshContent _mesh;
	private readonly float _positionX;
	private readonly float _positionZ;

	public TileMeshObject(uint vao, MeshContent mesh, float positionX, float positionZ)
	{
		_vao = vao;
		_mesh = mesh;
		_positionX = positionX;
		_positionZ = positionZ;
	}

	public float PositionY { get; set; }

	public unsafe void Render()
	{
		if (GlobalContext.Gl == null || GlobalContext.InternalResources == null)
			throw new InvalidOperationException();

		GL gl = GlobalContext.Gl;
		Shader meshShader = GlobalContext.InternalResources.MeshShader;

		Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(new(_positionX, PositionY, _positionZ));
		meshShader.SetUniform("model", translationMatrix);

		gl.BindVertexArray(_vao);
		fixed (uint* i = &_mesh.Indices[0])
			gl.DrawElements(PrimitiveType.Triangles, (uint)_mesh.Indices.Length, DrawElementsType.UnsignedInt, i);
		gl.BindVertexArray(0);
	}
}
