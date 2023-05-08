// ReSharper disable ForCanBeConvertedToForeach
using DevilDaggersInfo.App.Scenes.Base;
using DevilDaggersInfo.App.Scenes.Base.GameObjects;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes;

public sealed class ArenaScene : IArenaScene
{
	private readonly ArenaEditorContext? _editorContext;
	private readonly Func<SpawnsetBinary> _getSpawnset;
	private Player? _player;
	private Skull4? _skull4;

	public ArenaScene(Func<SpawnsetBinary> getSpawnset, bool useMenuCamera, bool isEditor)
	{
		_getSpawnset = getSpawnset;

		Camera = new(useMenuCamera)
		{
			PositionState = { Physics = new(0, 5, 0) },
		};

		IArenaScene scene = this;
		scene.InitializeArena();

		if (isEditor)
			_editorContext = new(this);
	}

	public Camera Camera { get; }
	public Tile[,] Tiles { get; } = new Tile[SpawnsetBinary.ArenaDimensionMax, SpawnsetBinary.ArenaDimensionMax];
	public List<LightObject> Lights { get; } = new();
	public RaceDagger RaceDagger { get; set; } = new();
	public int CurrentTick { get; set; }
	public ReplaySimulation? ReplaySimulation { get; private set; }

	public void AddSkull4()
	{
		_skull4 = new();
	}

	public void SetPlayerMovement(ReplaySimulation replaySimulation)
	{
		ReplaySimulation = replaySimulation;

		if (_player != null)
			Lights.Remove(_player.Light);

		_player = new(ReplaySimulation);
		Lights.Add(_player.Light);
	}

	public void Update(float delta)
	{
		SpawnsetBinary spawnset = _getSpawnset();

		IArenaScene scene = this;
		scene.FillArena(spawnset);

		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareUpdate();

		Camera.Update(delta);
		RaceDagger.Update(spawnset, CurrentTick);
		_player?.Update(CurrentTick);

		for (int i = 0; i < Tiles.GetLength(0); i++)
		{
			for (int j = 0; j < Tiles.GetLength(1); j++)
			{
				Tile tile = Tiles[i, j];
				tile.SetDisplayHeight(spawnset.GetActualTileHeight(tile.ArenaX, tile.ArenaY, CurrentTick / 60f));
			}
		}

		_editorContext?.Update(CurrentTick);
	}

	public void Render()
	{
		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareRender();

		Camera.PreRender();

		Shader shader = Root.InternalResources.MeshShader;
		shader.Use();
		shader.SetUniform("view", Camera.ViewMatrix);
		shader.SetUniform("projection", Camera.Projection);
		shader.SetUniform("textureDiffuse", 0);
		shader.SetUniform("textureLut", 1);
		shader.SetUniform("lutScale", 1f);

		// TODO: Prevent allocating memory?
		Span<Vector3> lightPositions = Lights.Select(lo => lo.PositionState.Render).ToArray();
		Span<Vector3> lightColors = Lights.Select(lo => lo.ColorState.Render).ToArray();
		Span<float> lightRadii = Lights.Select(lo => lo.RadiusState.Render).ToArray();

		shader.SetUniform("lightCount", lightPositions.Length);
		shader.SetUniform("lightPosition", lightPositions);
		shader.SetUniform("lightColor", lightColors);
		shader.SetUniform("lightRadius", lightRadii);

		Root.GameResources.PostLut.Bind(TextureUnit.Texture1);

		if (_editorContext != null && CurrentTick == 0)
			_editorContext.RenderTiles(shader);
		else
			RenderTilesDefault();

		RaceDagger.Render();
		_player?.Render();
		_skull4?.Render();

		Root.InternalResources.TileHitboxTexture.Bind();

		Span<Tile> tiles = Tiles.Cast<Tile>().ToArray(); // TODO: Prevent allocating memory.
		tiles.Sort(static (a, b) => a.SquaredDistanceToCamera().CompareTo(b.SquaredDistanceToCamera()));
		foreach (Tile tile in tiles)
			tile.RenderHitbox();
	}

	private void RenderTilesDefault()
	{
		Root.GameResources.TileTexture.Bind();

		for (int i = 0; i < Tiles.GetLength(0); i++)
		{
			for (int j = 0; j < Tiles.GetLength(1); j++)
				Tiles[i, j].RenderTop();
		}

		Root.GameResources.PillarTexture.Bind();

		for (int i = 0; i < Tiles.GetLength(0); i++)
		{
			for (int j = 0; j < Tiles.GetLength(1); j++)
				Tiles[i, j].RenderPillar();
		}
	}
}
