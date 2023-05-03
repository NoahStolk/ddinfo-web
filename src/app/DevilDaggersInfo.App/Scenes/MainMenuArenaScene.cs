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
		Camera = new(Root.Window, Root.InputContext)
		{
			IsMenuCamera = true,
		};

		IArenaScene scene = this;
		scene.AddArena(SpawnsetBinary.CreateDefault());

		_skull4 = new();
	}

	public Camera Camera { get; }
	public List<Tile> Tiles { get; } = new();
	public List<LightObject> Lights { get; } = new();
	public RaceDagger? RaceDagger { get; set; }

	public void Update(int currentTick, float delta)
	{
		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareUpdate();

		Camera.Update(delta);
		RaceDagger?.Update(currentTick);
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

		for (int i = 0; i < Tiles.Count; i++)
			Tiles[i].RenderTop();

		Root.GameResources.PillarTexture.Bind();
		for (int i = 0; i < Tiles.Count; i++)
			Tiles[i].RenderPillar();

		RaceDagger?.Render();
		_skull4.Render();

		Root.InternalResources.TileHitboxTexture.Bind();

		Tiles.Sort(static (a, b) => a.SquaredDistanceToCamera().CompareTo(b.SquaredDistanceToCamera()));
		foreach (Tile tile in Tiles)
			tile.RenderHitbox();
	}
}
