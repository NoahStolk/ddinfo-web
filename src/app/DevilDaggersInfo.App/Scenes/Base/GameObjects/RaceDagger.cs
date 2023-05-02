using DevilDaggersInfo.App.Engine.Content;
using DevilDaggersInfo.App.Engine.InterpolationStates;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes.Base.GameObjects;

public class RaceDagger
{
	private const float _yOffset = 4;

	private static uint _vao;

	private readonly Vector3State _meshPosition;
	private readonly QuaternionState _meshRotation;

	private readonly SpawnsetBinary _spawnsetBinary;

	private int _arenaDimension;
	private ImmutableArena _arenaTiles;
	private Vector2 _raceDaggerPosition;
	private int _arenaCoordX;
	private int _arenaCoordZ;

	public RaceDagger(SpawnsetBinary spawnsetBinary)
	{
		_spawnsetBinary = spawnsetBinary;

		_arenaDimension = _spawnsetBinary.ArenaDimension;
		_arenaTiles = _spawnsetBinary.ArenaTiles;
		_raceDaggerPosition = _spawnsetBinary.RaceDaggerPosition;
		(_arenaCoordX, _arenaCoordZ) = GetPosition();

		_meshPosition = new(new(_spawnsetBinary.RaceDaggerPosition.X, 0, _spawnsetBinary.RaceDaggerPosition.Y));
		_meshRotation = new(Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * 0.5f));
	}

	private (int ArenaCoordX, int ArenaCoordZ) GetPosition()
	{
		(int x, _, int z) = SpawnsetBinary.GetRaceDaggerTilePosition(_arenaDimension, _arenaTiles, _raceDaggerPosition);
		return (x, z);
	}

	public static unsafe void Initialize(GL gl)
	{
		_vao = CreateVao(gl, ContentManager.Content.DaggerMesh);

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

	/// <summary>
	/// Updates the position based on the tiles edited in the 3D editor. TODO: Also implement being able to update the race dagger position in the 3D editor.
	/// </summary>
	public void UpdatePosition(int arenaDimension, ImmutableArena arenaTiles, Vector2 raceDaggerPosition)
	{
		_arenaDimension = arenaDimension;
		_arenaTiles = arenaTiles;
		_raceDaggerPosition = raceDaggerPosition;
		(_arenaCoordX, _arenaCoordZ) = GetPosition();
	}

	public void Update(int currentTick)
	{
		_meshPosition.PrepareUpdate();
		_meshRotation.PrepareUpdate();

		float currentTime = currentTick / 60f;
		Vector3 basePosition = _meshPosition.Physics with
		{
			Y = SpawnsetBinary.GetActualTileHeight(_arenaDimension, _arenaTiles, _spawnsetBinary.ShrinkStart, _spawnsetBinary.ShrinkEnd, _spawnsetBinary.ShrinkRate, _arenaCoordX, _arenaCoordZ, currentTime) + _yOffset,
		};
		_meshPosition.Physics = basePosition + new Vector3(0, 0.15f + MathF.Sin(currentTime) * 0.15f, 0);
		_meshRotation.Physics = _meshRotation.Start * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, currentTime);
	}

	public unsafe void Render()
	{
		if (GlobalContext.Gl == null || GlobalContext.InternalResources == null || GlobalContext.GameResources == null)
			throw new InvalidOperationException();

		GL gl = GlobalContext.Gl;
		Shader meshShader = GlobalContext.InternalResources.MeshShader;

		_meshPosition.PrepareRender();
		_meshRotation.PrepareRender();

		GlobalContext.GameResources.DaggerSilverTexture.Bind();

		meshShader.SetUniform("model", Matrix4x4.CreateScale(8) * Matrix4x4.CreateFromQuaternion(_meshRotation.Render) * Matrix4x4.CreateTranslation(_meshPosition.Render));

		gl.BindVertexArray(_vao);
		fixed (uint* i = &ContentManager.Content.DaggerMesh.Indices[0])
			gl.DrawElements(PrimitiveType.Triangles, (uint)ContentManager.Content.DaggerMesh.Indices.Length, DrawElementsType.UnsignedInt, i);
		gl.BindVertexArray(0);
	}
}
