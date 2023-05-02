using DevilDaggersInfo.App.Engine.Content;
using DevilDaggersInfo.App.Ui.Base;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes.Base.GameObjects;

public class Tile
{
	private static uint _tileVao;
	private static uint _pillarVao;
	private static uint _cubeVao;

	private readonly Camera _camera;

	private readonly TileMeshObject _top;
	private readonly TileMeshObject _pillar;
	private readonly TileHitboxMeshObject _tileHitbox;

	public Tile(float positionX, float positionZ, int arenaX, int arenaY, Camera camera)
	{
		if (GlobalContext.InternalResources == null)
			throw new InvalidOperationException();

		PositionX = positionX;
		PositionZ = positionZ;
		ArenaX = arenaX;
		ArenaY = arenaY;
		_camera = camera;

		_top = new(_tileVao, ContentManager.Content.TileMesh, positionX, positionZ);
		_pillar = new(_pillarVao, ContentManager.Content.PillarMesh, positionX, positionZ);
		_tileHitbox = new(_cubeVao, GlobalContext.InternalResources.TileHitboxModel.MainMesh, positionX, positionZ);
	}

	public float PositionX { get; }
	public float Height { get; private set; }
	public float PositionZ { get; }
	public int ArenaX { get; }
	public int ArenaY { get; }

	public static unsafe void Initialize()
	{
		if (GlobalContext.Gl == null || GlobalContext.InternalResources == null)
			throw new InvalidOperationException();

		// TODO: Prevent this from being called multiple times.
		_tileVao = CreateVao(GlobalContext.Gl, ContentManager.Content.TileMesh);
		_pillarVao = CreateVao(GlobalContext.Gl, ContentManager.Content.PillarMesh);
		_cubeVao = CreateVao(GlobalContext.Gl, GlobalContext.InternalResources.TileHitboxModel.MainMesh);

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

	public float SquaredDistanceToCamera() => Vector2.DistanceSquared(new(PositionX, PositionZ), new(_camera.PositionState.Render.X, _camera.PositionState.Render.Z));

	public void SetDisplayHeight(float height)
	{
		Height = height;

		_top.PositionY = Height;
		_pillar.PositionY = Height;

		const float tileMeshHeight = 4;
		_tileHitbox.PositionY = Height - tileMeshHeight / 2;

		const float tileHitboxOffset = 1;
		_tileHitbox.Height = Height - tileMeshHeight / 2 + tileHitboxOffset;
	}

	public void RenderTop()
	{
		if (_top.PositionY < -3)
			return;

		_top.Render();
	}

	public void RenderPillar()
	{
		if (_top.PositionY < -3)
			return;

		_pillar.Render();
	}

	public void RenderHitbox()
	{
		if (_top.PositionY < -1)
			return;

		_tileHitbox.Render();
	}
}
