// ReSharper disable ForCanBeConvertedToForeach
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Scene.GameObjects;
using DevilDaggersInfo.Core.Replay.PostProcessing.PlayerMovement;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;

namespace DevilDaggersInfo.App.Ui.Scene;

public class ArenaScene
{
	private readonly Camera _camera = new();
	private readonly List<Tile> _tiles = new();
	private RaceDagger? _raceDagger;
	private Player? _player;

	public void BuildArena(SpawnsetBinary spawnset)
	{
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

	public void BuildPlayerMovement(PlayerMovementTimeline playerMovementTimeline)
	{
		_player = new(playerMovementTimeline);
	}

	public void Update(float currentTime)
	{
		_camera.Update();
		_raceDagger?.Update();
		_player?.Update(currentTime);

		for (int i = 0; i < _tiles.Count; i++)
			_tiles[i].Update(currentTime);
	}

	public void Render()
	{
		_camera.PreRender();

		WarpShaders.Mesh.Use();
		Shader.SetMatrix4x4(MeshUniforms.View, _camera.ViewMatrix);
		Shader.SetMatrix4x4(MeshUniforms.Projection, _camera.Projection);
		Shader.SetInt(MeshUniforms.TextureDiffuse, 0);

		ContentManager.Content.TileTexture.Use();
		for (int i = 0; i < _tiles.Count; i++)
			_tiles[i].RenderTop();

		ContentManager.Content.PillarTexture.Use();
		for (int i = 0; i < _tiles.Count; i++)
			_tiles[i].RenderPillar();

		_raceDagger?.Render();

		_player?.Render();
	}
}
