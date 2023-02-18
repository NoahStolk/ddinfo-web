// ReSharper disable ForCanBeConvertedToForeach
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Scene.GameObjects;
using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Silk.NET.OpenGL;
using Warp.NET.GameObjects.Common;

namespace DevilDaggersInfo.App.Ui.Scene;

public class ArenaScene
{
	private readonly Camera _camera = new();

	private readonly List<Tile> _tiles = new();
	private readonly List<LightObject> _lights = new();

	private RaceDagger? _raceDagger;
	private ReplaySimulation? _replaySimulation;
	private Player? _player;
	private Skull4? _skull4;

	private void Clear()
	{
		_tiles.Clear();
		_lights.Clear();

		_raceDagger = null;
		_replaySimulation = null;
		_player = null;
		_skull4 = null;
	}

	public void BuildMainMenu()
	{
		Clear();

		AddArena(SpawnsetBinary.CreateDefault());

		_camera.IsMenuCamera = true;
		_skull4 = new();
	}

	public void BuildSpawnset(SpawnsetBinary spawnset)
	{
		Clear();

		AddArena(spawnset);

		int halfSize = spawnset.ArenaDimension / 2;
		float cameraHeight = Math.Max(4, spawnset.ArenaTiles[halfSize, halfSize] + 4);
		_camera.Reset(new(0, cameraHeight, 0));
		_camera.IsMenuCamera = false;
	}

	private void AddArena(SpawnsetBinary spawnset)
	{
		_lights.Add(new(64, default, new(1, 0.5f, 0)));

		int halfSize = spawnset.ArenaDimension / 2;
		for (int i = 0; i < spawnset.ArenaDimension; i++)
		{
			for (int j = 0; j < spawnset.ArenaDimension; j++)
			{
				float y = spawnset.ArenaTiles[i, j];
				if (y < -2)
					continue;

				float x = (i - halfSize) * 4;
				float z = (j - halfSize) * 4;
				_tiles.Add(new(x, z, i, j, spawnset, _camera));
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
		Shader.SetInt(MeshUniforms.TextureLut, 1);
		Shader.SetFloat(MeshUniforms.LutScale, 1f);

		Span<Vector3> lightPositions = _lights.Select(lo => lo.PositionState.Render).ToArray();
		Span<Vector3> lightColors = _lights.Select(lo => lo.ColorState.Render).ToArray();
		Span<float> lightRadii = _lights.Select(lo => lo.RadiusState.Render).ToArray();

		Shader.SetInt(MeshUniforms.LightCount, lightPositions.Length);
		Shader.SetVector3Array(MeshUniforms.LightPosition, lightPositions);
		Shader.SetVector3Array(MeshUniforms.LightColor, lightColors);
		Shader.SetFloatArray(MeshUniforms.LightRadius, lightRadii);

		ContentManager.Content.PostLut.Use(TextureUnit.Texture1);

		ContentManager.Content.TileTexture.Use();
		for (int i = 0; i < _tiles.Count; i++)
			_tiles[i].RenderTop();

		ContentManager.Content.PillarTexture.Use();
		for (int i = 0; i < _tiles.Count; i++)
			_tiles[i].RenderPillar();

		_raceDagger?.Render();
		_player?.Render();
		_skull4?.Render();

		WarpTextures.TileHitbox.Use();

		_tiles.Sort(static (a, b) => a.SquaredDistanceToCamera().CompareTo(b.SquaredDistanceToCamera()));
		foreach (Tile tile in _tiles)
			tile.RenderHitbox();
	}
}
