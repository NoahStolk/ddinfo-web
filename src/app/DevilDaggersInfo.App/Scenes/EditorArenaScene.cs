// ReSharper disable ForCanBeConvertedToForeach
using DevilDaggersInfo.App.Engine.Intersections;
using DevilDaggersInfo.App.Scenes.Base;
using DevilDaggersInfo.App.Scenes.Base.GameObjects;
using DevilDaggersInfo.App.Ui.Main;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes;

public sealed class EditorArenaScene : IArenaScene
{
	private readonly List<(Tile Tile, float Distance)> _hitTiles = new();
	private Tile? _closestHitTile;

	public EditorArenaScene()
	{
		Camera = new(Root.Window, Root.InputContext, false)
		{
			PositionState = { Physics = new(0, 5, 0) },
		};

		IArenaScene scene = this;
		scene.FillArena(SpawnsetState.Spawnset);
	}

	public Camera Camera { get; }
	public Tile[,] Tiles { get; } = new Tile[SpawnsetBinary.ArenaDimensionMax, SpawnsetBinary.ArenaDimensionMax];
	public List<LightObject> Lights { get; } = new();
	public RaceDagger? RaceDagger { get; set; }

	public void Update(int currentTick, float delta)
	{
		IArenaScene scene = this;
		scene.FillArena(SpawnsetState.Spawnset);

		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareUpdate();

		Camera.Update(delta);
		RaceDagger?.Update(currentTick);

		for (int i = 0; i < Tiles.GetLength(0); i++)
		{
			for (int j = 0; j < Tiles.GetLength(1); j++)
			{
				Tile tile = Tiles[i, j];
				tile.SetDisplayHeight(SpawnsetState.Spawnset.GetActualTileHeight(tile.ArenaX, tile.ArenaY, currentTick / 60f));
			}
		}

		float scroll = Root.InputContext.Mice[0].ScrollWheels[0].Y;
		if (currentTick > 0 || scroll == 0 || _closestHitTile == null)
			return;

		float height = SpawnsetState.Spawnset.ArenaTiles[_closestHitTile.ArenaX, _closestHitTile.ArenaY] - scroll;
		_closestHitTile.SetDisplayHeight(height);

		float[,] newArena = SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		newArena[_closestHitTile.ArenaX, _closestHitTile.ArenaY] = height;
		SpawnsetState.Spawnset = SpawnsetState.Spawnset with { ArenaTiles = new(SpawnsetState.Spawnset.ArenaDimension, newArena) };
		SpawnsetHistoryUtils.Save(SpawnsetEditType.ArenaTileHeight);

		int dimension = SpawnsetState.Spawnset.ArenaDimension;
		RaceDagger?.UpdatePosition(dimension, new(dimension, newArena), SpawnsetState.Spawnset.RaceDaggerPosition);
	}

	public void Render(int currentTick)
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

		RenderTiles(shader, currentTick);

		RaceDagger?.Render();

		Root.InternalResources.TileHitboxTexture.Bind();

		Span<Tile> tiles = Tiles.Cast<Tile>().ToArray();
		tiles.Sort(static (a, b) => a.SquaredDistanceToCamera().CompareTo(b.SquaredDistanceToCamera()));
		foreach (Tile tile in tiles)
			tile.RenderHitbox();
	}

	private void RenderTiles(Shader shader, int currentTick)
	{
		if (currentTick == 0)
		{
			_hitTiles.Clear();
			Ray ray = Camera.ScreenToWorldPoint();
			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					Tile tile = Tiles[i, j];
					Vector3 min = new(tile.PositionX - 2, -2, tile.PositionZ - 2);
					Vector3 max = new(tile.PositionX + 2, tile.Height + 2, tile.PositionZ + 2);
					RayVsAabbIntersection? intersects = ray.Intersects(min, max);
					if (intersects.HasValue)
						_hitTiles.Add((tile, intersects.Value.Distance));
				}
			}

			_closestHitTile = _hitTiles.Count == 0 ? null : _hitTiles.MinBy(ht => ht.Distance).Tile;

			// Temporarily use LutScale to highlight the target tile.
			Root.GameResources.TileTexture.Bind();

			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					Tile tile = Tiles[i, j];

					if (tile.Height < -2)
						continue;

					if (_closestHitTile == tile)
						shader.SetUniform("lutScale", 2.5f);

					tile.RenderTop();

					if (_closestHitTile == tile)
						shader.SetUniform("lutScale", 1f);
				}
			}

			Root.GameResources.PillarTexture.Bind();

			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					Tile tile = Tiles[i, j];

					if (tile.Height < -2)
						continue;

					if (_closestHitTile == tile)
						shader.SetUniform("lutScale", 2.5f);

					tile.RenderPillar();

					if (_closestHitTile == tile)
						shader.SetUniform("lutScale", 1f);
				}
			}
		}
		else
		{
			Root.GameResources.TileTexture.Bind();

			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					Tile tile = Tiles[i, j];
					if (tile.Height < -2)
						continue;

					tile.RenderTop();
				}
			}

			Root.GameResources.PillarTexture.Bind();

			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					if (Tiles[i, j].Height < -2)
						continue;

					Tiles[i, j].RenderPillar();
				}
			}
		}
	}
}
