// ReSharper disable ForCanBeConvertedToForeach
using DevilDaggersInfo.App.Scenes.Base;
using DevilDaggersInfo.App.Scenes.Base.GameObjects;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes;

public sealed class MainMenuArenaScene : IArenaScene
{
	private readonly Skull4 _skull4;

	public MainMenuArenaScene()
	{
		Camera = new(true);

		IArenaScene scene = this;
		scene.FillArena(SpawnsetBinary.CreateDefault());

		_skull4 = new();
	}

	public Camera Camera { get; }
	public Tile[,] Tiles { get; } = new Tile[SpawnsetBinary.ArenaDimensionMax, SpawnsetBinary.ArenaDimensionMax];
	public List<LightObject> Lights { get; } = new();
	public RaceDagger? RaceDagger { get; set; }

	public void Update(int currentTick, float delta)
	{
		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareUpdate();

		Camera.Update(delta);
		RaceDagger?.Update(currentTick);
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

		RaceDagger?.Render();
		_skull4.Render();

		Root.InternalResources.TileHitboxTexture.Bind();

		Span<Tile> tiles = Tiles.Cast<Tile>().ToArray();
		tiles.Sort(static (a, b) => a.SquaredDistanceToCamera().CompareTo(b.SquaredDistanceToCamera()));
		foreach (Tile tile in tiles)
			tile.RenderHitbox();
	}
}
