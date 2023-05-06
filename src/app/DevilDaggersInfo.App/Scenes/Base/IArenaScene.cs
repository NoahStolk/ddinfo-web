using DevilDaggersInfo.App.Scenes.Base.GameObjects;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Scenes.Base;

public interface IArenaScene
{
	public const float MinRenderTileHeight = -3;

	Camera Camera { get; }
	Tile[,] Tiles { get; }
	List<LightObject> Lights { get; }
	RaceDagger? RaceDagger { get; set; }
	int CurrentTick { get; }

	void Update(float delta);

	void Render();

	public void InitializeArena()
	{
		const int halfSize = SpawnsetBinary.ArenaDimensionMax / 2;
		for (int i = 0; i < SpawnsetBinary.ArenaDimensionMax; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimensionMax; j++)
			{
				float x = (i - halfSize) * 4;
				float z = (j - halfSize) * 4;
				Tiles[i, j] = new(x, z, i, j, Camera);
			}
		}
	}

	public void FillArena(SpawnsetBinary spawnset)
	{
		Lights.Clear();
		Lights.Add(new(64, default, new(1, 0.5f, 0)));

		for (int i = 0; i < spawnset.ArenaDimension; i++)
		{
			for (int j = 0; j < spawnset.ArenaDimension; j++)
				Tiles[i, j].SetDisplayHeight(spawnset.ArenaTiles[i, j]);
		}

		RaceDagger = GetRaceDagger();

		RaceDagger? GetRaceDagger()
		{
			if (spawnset.GameMode != GameMode.Race)
				return null;

			return new(spawnset);
		}
	}
}
