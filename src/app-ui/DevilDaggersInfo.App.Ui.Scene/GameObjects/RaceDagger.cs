using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.OpenGL;
using Warp.NET.Content;
using Warp.NET.InterpolationStates;

namespace DevilDaggersInfo.App.Ui.Scene.GameObjects;

public class RaceDagger
{
	private const float _yOffset = 4;

	private static uint _vao;

	private readonly SpawnsetBinary _spawnset;
	private readonly int _arenaCoordX;
	private readonly int _arenaCoordZ;
	private readonly float _worldX;
	private readonly float _worldZ;
	private readonly Vector3State _position;
	private readonly QuaternionState _rotation;

	public RaceDagger(SpawnsetBinary spawnset, int arenaCoordX, float y, int arenaCoordZ)
	{
		_spawnset = spawnset;
		_arenaCoordX = arenaCoordX;
		_arenaCoordZ = arenaCoordZ;
		_worldX = spawnset.TileToWorldCoordinate(arenaCoordX);
		_worldZ = spawnset.TileToWorldCoordinate(arenaCoordZ);
		_position = new(new(_worldX, y + _yOffset, _worldZ));
		_rotation = new(Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * 0.5f));
	}

	public static unsafe void Initialize()
	{
		_vao = CreateVao(ContentManager.Content.DaggerMesh);

		static uint CreateVao(Mesh mesh)
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

	public void Update(int currentTick)
	{
		_position.PrepareUpdate();
		_rotation.PrepareUpdate();

		float currentTime = currentTick / 60f;
		Vector3 basePosition = new(_worldX, _spawnset.GetActualTileHeight(_arenaCoordX, _arenaCoordZ, currentTime) + _yOffset, _worldZ);
		_position.Physics = basePosition + new Vector3(0, 0.15f + MathF.Sin(currentTime) * 0.15f, 0);
		_rotation.Physics = _rotation.Start * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, currentTime);
	}

	public unsafe void Render()
	{
		_position.PrepareRender();
		_rotation.PrepareRender();

		ContentManager.Content.DaggerSilverTexture.Use();

		Matrix4x4 model = Matrix4x4.CreateScale(8) * Matrix4x4.CreateFromQuaternion(_rotation.Render) * Matrix4x4.CreateTranslation(_position.Render);
		Shader.SetMatrix4x4(MeshUniforms.Model, model);

		Gl.BindVertexArray(_vao);
		fixed (uint* i = &ContentManager.Content.DaggerMesh.Indices[0])
			Gl.DrawElements(PrimitiveType.Triangles, (uint)ContentManager.Content.DaggerMesh.Indices.Length, DrawElementsType.UnsignedInt, i);
		Gl.BindVertexArray(0);
	}
}