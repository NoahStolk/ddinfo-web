using DevilDaggersInfo.App.Ui.Base;
using Silk.NET.OpenGL;
using Warp.NET.Content;

namespace DevilDaggersInfo.App.Ui.Scene.GameObjects;

public class TileMeshObject
{
	private readonly uint _vao;
	private readonly Mesh _mesh;
	private readonly Matrix4x4 _scaleRotationMatrix;
	private readonly float _positionX;
	private readonly float _positionZ;

	public TileMeshObject(uint vao, Mesh mesh, Vector3 scale, Quaternion rotation, float positionX, float positionZ)
	{
		_vao = vao;
		_mesh = mesh;
		_positionX = positionX;
		_positionZ = positionZ;

		Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scale);
		Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotation);
		_scaleRotationMatrix = scaleMatrix * rotationMatrix;
	}

	public float PositionY { get; set; }

	public unsafe void Render()
	{
		Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(new(_positionX, PositionY, _positionZ));
		Shader.SetMatrix4x4(MeshUniforms.Model, _scaleRotationMatrix * translationMatrix);

		Gl.BindVertexArray(_vao);
		fixed (uint* i = &_mesh.Indices[0])
			Gl.DrawElements(PrimitiveType.Triangles, (uint)_mesh.Indices.Length, DrawElementsType.UnsignedInt, i);
		Gl.BindVertexArray(0);
	}
}
