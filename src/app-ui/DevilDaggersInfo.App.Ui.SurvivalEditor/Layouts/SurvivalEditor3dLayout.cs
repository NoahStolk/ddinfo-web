using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.Scene.GameObjects;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Warp.NET;
using Warp.NET.Content;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditor3dLayout : Layout, ISurvivalEditor3dLayout
{
	private readonly ShrinkSlider _shrinkSlider;

	private readonly Camera _camera = new();
	private readonly List<Tile> _tiles = new();
	private RaceDagger? _raceDagger;

	private float _currentTime;

	public SurvivalEditor3dLayout()
	{
		_shrinkSlider = new(new PixelBounds(0, 752, 1024, 16), f => _currentTime = f, true, 0, 0, 0.1f, 0, GlobalStyles.DefaultSliderStyle);
		NestingContext.Add(_shrinkSlider);
	}

	public void BuildScene(SpawnsetBinary spawnset)
	{
		_currentTime = 0;

		_shrinkSlider.Max = spawnset.GetSliderMaxSeconds();
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);

		_tiles.Clear();

		int halfSize = spawnset.ArenaDimension / 2;
		_camera.Reset(new(0, spawnset.ArenaTiles[halfSize, halfSize] + 4, 0));

		for (int i = 0; i < spawnset.ArenaDimension; i++)
		{
			for (int j = 0; j < spawnset.ArenaDimension; j++)
			{
				float y = spawnset.ArenaTiles[i, j];
				if (y < -2)
					continue;

				float x = (i - halfSize) * 4;
				float z = (j - halfSize) * 4;
				_tiles.Add(new(x, z, i, j, spawnset));
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
		_currentTime += Root.Game.Dt;
		_shrinkSlider.CurrentValue = _currentTime;

		_camera.Update();
		_raceDagger?.Update();

		foreach (Tile tile in _tiles)
			tile.Update(_currentTime);

		if (Input.IsKeyPressed(Keys.Escape))
			LayoutManager.ToSurvivalEditorMainLayout();
	}

	public void Render3d()
	{
		_camera.PreRender();

		WarpShaders.Mesh.Use();
		Shader.SetMatrix4x4(MeshUniforms.View, _camera.ViewMatrix);
		Shader.SetMatrix4x4(MeshUniforms.Projection, _camera.Projection);
		Shader.SetInt(MeshUniforms.TextureDiffuse, 0);

		ContentManager.Content.TileTexture.Use();
		foreach (Tile tile in _tiles)
			tile.RenderTop();

		ContentManager.Content.PillarTexture.Use();
		foreach (Tile tile in _tiles)
			tile.RenderPillar();

		_raceDagger?.Render();
	}

	public void Render()
	{
	}
}
