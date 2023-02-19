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
	private ReplaySimulation? _replaySimulation;
	private Player? _player;
	private Skull4? _skull4;

	protected Camera Camera { get; } = new();
	protected List<Tile> Tiles { get; } = new();
	protected List<LightObject> Lights { get; } = new();
	protected RaceDagger? RaceDagger { get; private set; }

	private void Clear()
	{
		Tiles.Clear();
		Lights.Clear();

		RaceDagger = null;
		_replaySimulation = null;
		_player = null;
		_skull4 = null;
	}

	public void BuildMainMenu()
	{
		Clear();

		AddArena(SpawnsetBinary.CreateDefault());

		Camera.IsMenuCamera = true;
		_skull4 = new();
	}

	public void BuildSpawnset(SpawnsetBinary spawnset)
	{
		Clear();

		AddArena(spawnset);

		int halfSize = spawnset.ArenaDimension / 2;
		float cameraHeight = Math.Max(4, spawnset.ArenaTiles[halfSize, halfSize] + 4);
		Camera.Reset(new(0, cameraHeight, 0));
		Camera.IsMenuCamera = false;
	}

	private void AddArena(SpawnsetBinary spawnset)
	{
		Lights.Add(new(64, default, new(1, 0.5f, 0)));

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
				Tiles.Add(new(x, z, i, j, spawnset, Camera));
			}
		}

		RaceDagger = GetRaceDagger();

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
		Lights.Add(_player.Light);
	}

	public virtual void Update(int currentTick)
	{
		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareUpdate();

		Camera.Update();
		RaceDagger?.Update(currentTick);
		_player?.Update(currentTick);

		for (int i = 0; i < Tiles.Count; i++)
			Tiles[i].Update(currentTick);

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

	public virtual void Render()
	{
		for (int i = 0; i < Lights.Count; i++)
			Lights[i].PrepareRender();

		Camera.PreRender();

		WarpShaders.Mesh.Use();
		Shader.SetMatrix4x4(MeshUniforms.View, Camera.ViewMatrix);
		Shader.SetMatrix4x4(MeshUniforms.Projection, Camera.Projection);
		Shader.SetInt(MeshUniforms.TextureDiffuse, 0);
		Shader.SetInt(MeshUniforms.TextureLut, 1);
		Shader.SetFloat(MeshUniforms.LutScale, 1f);

		Span<Vector3> lightPositions = Lights.Select(lo => lo.PositionState.Render).ToArray();
		Span<Vector3> lightColors = Lights.Select(lo => lo.ColorState.Render).ToArray();
		Span<float> lightRadii = Lights.Select(lo => lo.RadiusState.Render).ToArray();

		Shader.SetInt(MeshUniforms.LightCount, lightPositions.Length);
		Shader.SetVector3Array(MeshUniforms.LightPosition, lightPositions);
		Shader.SetVector3Array(MeshUniforms.LightColor, lightColors);
		Shader.SetFloatArray(MeshUniforms.LightRadius, lightRadii);

		ContentManager.Content.PostLut.Use(TextureUnit.Texture1);

		ContentManager.Content.TileTexture.Use();

		for (int i = 0; i < Tiles.Count; i++)
			Tiles[i].RenderTop();

		ContentManager.Content.PillarTexture.Use();
		for (int i = 0; i < Tiles.Count; i++)
			Tiles[i].RenderPillar();

		RaceDagger?.Render();
		_player?.Render();
		_skull4?.Render();

		WarpTextures.TileHitbox.Use();

		Tiles.Sort(static (a, b) => a.SquaredDistanceToCamera().CompareTo(b.SquaredDistanceToCamera()));
		foreach (Tile tile in Tiles)
			tile.RenderHitbox();
	}
}
