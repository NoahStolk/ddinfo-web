using Silk.NET.OpenGL;
using Warp.NET.Content;

namespace DevilDaggersInfo.App.Ui.Base.GameObjects;

public class StaticMeshObject
{
	private readonly uint _vao;
	private readonly Mesh _mesh;
	private readonly Matrix4x4 _modelMatrix;

	public StaticMeshObject(uint vao, Mesh mesh, Vector3 scale, Quaternion rotation, Vector3 position)
	{
		_vao = vao;
		_mesh = mesh;

		Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scale);
		Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotation);
		Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(position);
		_modelMatrix = scaleMatrix * rotationMatrix * translationMatrix;
	}

	public unsafe void Render()
	{
		Shader.SetMatrix4x4(MeshUniforms.Model, _modelMatrix);

		Gl.BindVertexArray(_vao);
		fixed (uint* i = &_mesh.Indices[0])
			Gl.DrawElements(PrimitiveType.Triangles, (uint)_mesh.Indices.Length, DrawElementsType.UnsignedInt, i);
		Gl.BindVertexArray(0);
	}
}
