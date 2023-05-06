// ReSharper disable ForCanBeConvertedToForeach
using DevilDaggersInfo.App.Scenes.Base;
using DevilDaggersInfo.App.Scenes.Base.GameObjects;
using DevilDaggersInfo.App.Ui.ReplayEditor.State;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes;

public sealed class ReplayArenaScene : IArenaScene
{
	private Player? _player;

	public ReplayArenaScene()
	{
		Camera = new(false)
		{
			PositionState = { Physics = new(0, 5, 0) },
		};

		IArenaScene scene = this;
		scene.InitializeArena();
	}

	public Camera Camera { get; }
	public Tile[,] Tiles { get; } = new Tile[SpawnsetBinary.ArenaDimensionMax, SpawnsetBinary.ArenaDimensionMax];
	public List<LightObject> Lights { get; } = new();
	public RaceDagger? RaceDagger { get; set; }
	public int CurrentTick => 0; // TODO: Implement this.

	// TODO: Implement this.
	public void BuildPlayerMovement(ReplaySimulation replaySimulation)
	{
		_player = new(replaySimulation);
		Lights.Add(_player.Light);
	}

	public void Update(float delta)
	{
		IArenaScene scene = this;
		scene.FillArena(ReplayState.Replay.Header.Spawnset);

		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareUpdate();

		Camera.Update(delta);
		RaceDagger?.Update(CurrentTick);
		_player?.Update(CurrentTick);

		for (int i = 0; i < Tiles.GetLength(0); i++)
		{
			for (int j = 0; j < Tiles.GetLength(1); j++)
			{
				Tile tile = Tiles[i, j];
				tile.SetDisplayHeight(ReplayState.Replay.Header.Spawnset.GetActualTileHeight(tile.ArenaX, tile.ArenaY, CurrentTick / 60f));
			}
		}
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

		Root.GameResources.TileTexture.Bind();

		for (int i = 0; i < Tiles.GetLength(0); i++)
		{
			for (int j = 0; j < Tiles.GetLength(1); j++)
			{
				Tile tile = Tiles[i, j];
				if (tile.Height < IArenaScene.MinRenderTileHeight)
					continue;

				tile.RenderTop();
			}
		}

		Root.GameResources.PillarTexture.Bind();

		for (int i = 0; i < Tiles.GetLength(0); i++)
		{
			for (int j = 0; j < Tiles.GetLength(1); j++)
			{
				if (Tiles[i, j].Height < IArenaScene.MinRenderTileHeight)
					continue;

				Tiles[i, j].RenderPillar();
			}
		}

		RaceDagger?.Render();
		_player?.Render();

		Root.InternalResources.TileHitboxTexture.Bind();

		Span<Tile> tiles = Tiles.Cast<Tile>().ToArray(); // TODO: Prevent allocating memory.
		tiles.Sort(static (a, b) => a.SquaredDistanceToCamera().CompareTo(b.SquaredDistanceToCamera()));
		foreach (Tile tile in tiles)
			tile.RenderHitbox();
	}
}
