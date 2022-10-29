using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.GameObjects;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Warp;
using Warp.Content;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditor3dLayout : Layout, ISurvivalEditor3dLayout
{
	private readonly Camera _camera = new();
	private readonly List<MeshObject> _tiles = new();
	private readonly List<MeshObject> _pillars = new();
	private RaceDagger? _raceDagger;

	private uint _tileVao;
	private uint _pillarVao;

	public SurvivalEditor3dLayout()
		: base(Constants.Full)
	{
	}

	public unsafe void Initialize()
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

	public void BuildScene()
	{
		_tiles.Clear();
		_pillars.Clear();

		SpawnsetBinary spawnset = StateManager.SpawnsetState.Spawnset;
		int halfSize = spawnset.ArenaDimension / 2;
		for (int i = 0; i < spawnset.ArenaDimension; i++)
		{
			for (int j = 0; j < spawnset.ArenaDimension; j++)
			{
				float y = spawnset.ArenaTiles[i, j];
				if (y < -2)
					continue;

				float x = (i - halfSize) * 4;
				float z = (j - halfSize) * 4;
				_tiles.Add(new(_tileVao, ContentManager.Content.TileMesh, Vector3.One, Quaternion.Identity, new(x, y, z)));
				_pillars.Add(new(_pillarVao, ContentManager.Content.PillarMesh, Vector3.One, Quaternion.Identity, new(x, y, z)));
			}
		}

		_raceDagger = GetRaceDagger();

		RaceDagger? GetRaceDagger()
		{
			if (spawnset.GameMode != GameMode.Race)
				return null;

			(int x, float? y, int z) = spawnset.GetRaceDaggerTilePosition();
			if (!y.HasValue)
				return null;

			return new(new(spawnset.TileToWorldCoordinate(x), y.Value + 4, spawnset.TileToWorldCoordinate(z)));
		}
	}

	public void Update()
	{
		_camera.Update();
		_raceDagger?.Update();

		if (Input.IsKeyPressed(Keys.Escape))
			LayoutManager.ToSurvivalEditorMainLayout();
	}

	public void Render3d()
	{
		_camera.PreRender();

		Shaders.Mesh.Use();
		Shader.SetMatrix4x4(MeshUniforms.View, _camera.ViewMatrix);
		Shader.SetMatrix4x4(MeshUniforms.Projection, _camera.Projection);
		Shader.SetInt(MeshUniforms.TextureDiffuse, 0);

		ContentManager.Content.TileTexture.Use();
		foreach (MeshObject tile in _tiles)
			tile.Render();

		ContentManager.Content.PillarTexture.Use();
		foreach (MeshObject pillar in _pillars)
			pillar.Render();

		_raceDagger?.Render();
	}

	public void Render()
	{
	}
}
