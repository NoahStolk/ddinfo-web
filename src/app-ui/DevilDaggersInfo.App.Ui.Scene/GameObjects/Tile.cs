using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.OpenGL;
using Warp.NET.Content;
using Warp.NET.GameObjects;

namespace DevilDaggersInfo.App.Ui.Scene.GameObjects;

public class Tile : GameObject
{
	private static uint _tileVao;
	private static uint _pillarVao;

	private readonly int _arenaX;
	private readonly int _arenaY;
	private readonly SpawnsetBinary _spawnsetBinary;

	private readonly TileMeshObject _top;
	private readonly TileMeshObject _pillar;

	public Tile(float positionX, float positionZ, int arenaX, int arenaY, SpawnsetBinary spawnsetBinary)
	{
		_arenaX = arenaX;
		_arenaY = arenaY;
		_spawnsetBinary = spawnsetBinary;

		_top = new(_tileVao, ContentManager.Content.TileMesh, Vector3.One, Quaternion.Identity, positionX, positionZ);
		_pillar = new(_pillarVao, ContentManager.Content.PillarMesh, Vector3.One, Quaternion.Identity, positionX, positionZ);
	}

	public static unsafe void Initialize()
	{
		_tileVao = CreateVao(ContentManager.Content.TileMesh);
		_pillarVao = CreateVao(ContentManager.Content.PillarMesh);

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

	public void Update(float currentTime)
	{
		float y = _spawnsetBinary.GetActualTileHeight(_arenaX, _arenaY, currentTime);
		_top.PositionY = y;
		_pillar.PositionY = y;
	}

	public void RenderTop()
	{
		_top.Render();
	}

	public void RenderPillar()
	{
		_pillar.Render();
	}
}
