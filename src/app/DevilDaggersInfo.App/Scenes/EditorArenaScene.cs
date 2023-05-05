// ReSharper disable ForCanBeConvertedToForeach
using DevilDaggersInfo.App.Engine.Intersections;
using DevilDaggersInfo.App.Scenes.Base;
using DevilDaggersInfo.App.Scenes.Base.GameObjects;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
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
		Camera = new(Root.Window, Root.InputContext);
	}

	public Camera Camera { get; }
	public List<Tile> Tiles { get; } = new();
	public List<LightObject> Lights { get; } = new();
	public RaceDagger? RaceDagger { get; set; }

	private void Clear()
	{
		Tiles.Clear();
		Lights.Clear();

		RaceDagger = null;
	}

	// TODO: Make immediate.
	public void BuildSpawnset(SpawnsetBinary spawnset)
	{
		Clear();

		IArenaScene scene = this;
		scene.AddArena(spawnset);

		int halfSize = spawnset.ArenaDimension / 2;
		float cameraHeight = Math.Max(4, spawnset.ArenaTiles[halfSize, halfSize] + 8);
		Camera.Reset(new(0, cameraHeight, 0));
		Camera.IsMenuCamera = false;
	}

	public void Update(int currentTick)
	{
		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareUpdate();

		Camera.Update(currentTick / 60f);
		RaceDagger?.Update(currentTick);

		for (int i = 0; i < Tiles.Count; i++)
		{
			Tile tile = Tiles[i];
			tile.SetDisplayHeight(SpawnsetState.Spawnset.GetActualTileHeight(tile.ArenaX, tile.ArenaY, currentTick / 60f));
		}

		ImGuiIOPtr io = ImGui.GetIO();
		float scroll = io.MouseWheel;
		if (currentTick > 0 || scroll == 0 || _closestHitTile == null)
			return;

		float height = SpawnsetState.Spawnset.ArenaTiles[_closestHitTile.ArenaX, _closestHitTile.ArenaY] - scroll;
		_closestHitTile.SetDisplayHeight(height);

		float[,] newArena = SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		newArena[_closestHitTile.ArenaX, _closestHitTile.ArenaY] = height;
		//StateManager.Dispatch(new UpdateArena(newArena, SpawnsetEditType.ArenaTileHeight));

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

		Tiles.Sort(static (a, b) => a.SquaredDistanceToCamera().CompareTo(b.SquaredDistanceToCamera()));
		foreach (Tile tile in Tiles)
			tile.RenderHitbox();
	}

	private void RenderTiles(Shader shader, int currentTick)
	{
		if (currentTick == 0)
		{
			_hitTiles.Clear();
			Ray ray = Camera.ScreenToWorldPoint();
			for (int i = 0; i < Tiles.Count; i++)
			{
				Tile tile = Tiles[i];
				Vector3 min = new(tile.PositionX - 2, -2, tile.PositionZ - 2);
				Vector3 max = new(tile.PositionX + 2, tile.Height + 2, tile.PositionZ + 2);
				RayVsAabbIntersection? intersects = ray.Intersects(min, max);
				if (intersects.HasValue)
					_hitTiles.Add((tile, intersects.Value.Distance));
			}

			_closestHitTile = _hitTiles.Count == 0 ? null : _hitTiles.MinBy(ht => ht.Distance).Tile;

			// Temporarily use LutScale to highlight the target tile.
			Root.GameResources.TileTexture.Bind();
			for (int i = 0; i < Tiles.Count; i++)
			{
				Tile tile = Tiles[i];

				if (_closestHitTile == tile)
					shader.SetUniform("lutScale", 2.5f);

				tile.RenderTop();

				if (_closestHitTile == tile)
					shader.SetUniform("lutScale", 1f);
			}

			Root.GameResources.PillarTexture.Bind();
			for (int i = 0; i < Tiles.Count; i++)
			{
				Tile tile = Tiles[i];

				if (_closestHitTile == tile)
					shader.SetUniform("lutScale", 2.5f);

				tile.RenderPillar();

				if (_closestHitTile == tile)
					shader.SetUniform("lutScale", 1f);
			}
		}
		else
		{
			Root.GameResources.TileTexture.Bind();
			for (int i = 0; i < Tiles.Count; i++)
				Tiles[i].RenderTop();

			Root.GameResources.PillarTexture.Bind();
			for (int i = 0; i < Tiles.Count; i++)
				Tiles[i].RenderPillar();
		}
	}
}
