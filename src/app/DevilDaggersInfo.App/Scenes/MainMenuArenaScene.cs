// ReSharper disable ForCanBeConvertedToForeach
using DevilDaggersInfo.App.Scenes.Base;
using DevilDaggersInfo.App.Scenes.Base.GameObjects;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes;

public sealed class MainMenuArenaScene : IArenaScene
{
	private Skull4? _skull4;

	public MainMenuArenaScene()
	{
		// TODO: These should only be called once.
		Skull4.Initialize();
		Tile.Initialize();

		if (GlobalContext.Window == null || GlobalContext.InputContext == null)
			throw new();

		Camera = new(GlobalContext.Window, GlobalContext.InputContext);
	}

	public Camera Camera { get; }
	public List<Tile> Tiles { get; } = new();
	public List<LightObject> Lights { get; } = new();
	public RaceDagger? RaceDagger { get; set; }

	private void Clear()
	{
		_skull4 = null;

		Tiles.Clear();
		Lights.Clear();

		RaceDagger = null;
	}

	public void BuildSpawnset(SpawnsetBinary spawnset)
	{
		Clear();

		IArenaScene scene = this;
		scene.AddArena(spawnset);

		Camera.IsMenuCamera = true;
		_skull4 = new();
	}

	public void Update(int currentTick, float delta)
	{
		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareUpdate();

		Camera.Update(delta);
		RaceDagger?.Update(currentTick);
	}

	public void Render()
	{
		if (GlobalContext.Gl == null || GlobalContext.InternalResources == null || GlobalContext.GameResources == null)
			throw new();

		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareRender();

		Camera.PreRender();

		Shader shader = GlobalContext.InternalResources.MeshShader;
		shader.Use();
		shader.SetUniform("view", Camera.ViewMatrix);
		shader.SetUniform("projection", Camera.Projection);
		shader.SetUniform("textureDiffuse", 0);
		shader.SetUniform("textureLut", 1);
		shader.SetUniform("lutScale", 1f);

		// TODO: Prevent allocating memory.
		Span<Vector3> lightPositions = Lights.Select(lo => lo.PositionState.Render).ToArray();
		Span<Vector3> lightColors = Lights.Select(lo => lo.ColorState.Render).ToArray();
		Span<float> lightRadii = Lights.Select(lo => lo.RadiusState.Render).ToArray();

		shader.SetUniform("lightCount", lightPositions.Length);
		shader.SetUniform("lightPosition", lightPositions);
		shader.SetUniform("lightColor", lightColors);
		shader.SetUniform("lightRadius", lightRadii);

		GlobalContext.GameResources.PostLut.Bind(TextureUnit.Texture1);

		GlobalContext.GameResources.TileTexture.Bind();

		for (int i = 0; i < Tiles.Count; i++)
			Tiles[i].RenderTop();

		GlobalContext.GameResources.PillarTexture.Bind();
		for (int i = 0; i < Tiles.Count; i++)
			Tiles[i].RenderPillar();

		RaceDagger?.Render();
		_skull4?.Render();

		GlobalContext.InternalResources.TileHitboxTexture.Bind();

		Tiles.Sort(static (a, b) => a.SquaredDistanceToCamera().CompareTo(b.SquaredDistanceToCamera()));
		foreach (Tile tile in Tiles)
			tile.RenderHitbox();
	}
}
