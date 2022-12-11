// ReSharper disable ForCanBeConvertedToForeach
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Scene.GameObjects;
using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Warp.NET.GameObjects.Common;

namespace DevilDaggersInfo.App.Ui.Scene;

public class ArenaScene
{
	private readonly Camera _camera = new();
	private readonly List<Tile> _tiles = new();
	private RaceDagger? _raceDagger;
	private ReplaySimulation? _replaySimulation;
	private Player? _player;

	public void BuildArena(SpawnsetBinary spawnset)
	{
		_tiles.Clear();
		_player = null;

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

			return new(spawnset, x, y.Value, z);
		}
	}

	public void BuildPlayerMovement(ReplaySimulation replaySimulation)
	{
		_replaySimulation = replaySimulation;
		_player = new(replaySimulation);
	}

	public void Update(int currentTick)
	{
		_camera.Update();
		_raceDagger?.Update(currentTick);
		_player?.Update(currentTick);

		for (int i = 0; i < _tiles.Count; i++)
			_tiles[i].Update(currentTick);

		SoundSnapshot[] soundSnapshots = _replaySimulation?.GetSoundSnapshots(currentTick) ?? Array.Empty<SoundSnapshot>();
		foreach (SoundSnapshot soundSnapshot in soundSnapshots)
		{
			Sound3dObject sound = new(soundSnapshot.Sound switch
			{
				ReplaySound.Jump1 => ContentManager.Content.SoundJump1,
				ReplaySound.Jump2 => ContentManager.Content.SoundJump2,
				ReplaySound.Jump3 => ContentManager.Content.SoundJump3,
				_ => throw new InvalidEnumConversionException(soundSnapshot.Sound),
			})
			{
				Position = soundSnapshot.Position,
			};
			sound.Add();
		}
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

		WarpTextures.TileHitbox.Use();

		Span<Tile> tiles = _tiles.OrderBy(t => Vector3.DistanceSquared(t.Position with { Y = _camera.PositionState.Render.Y }, _camera.PositionState.Render)).ToArray();
		for (int i = 0; i < tiles.Length; i++)
			tiles[i].RenderHitbox();
	}
}
