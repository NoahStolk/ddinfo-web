using System.Numerics;

namespace DevilDaggersInfo.Core.Spawnset;

public static class SpawnsetUtils
{
	public static float? GetRaceDaggerHeight(int arenaDimension, ImmutableArena arenaTiles, int raceDaggerTileX, int raceDaggerTileZ)
	{
		if (raceDaggerTileX >= 0 && raceDaggerTileX < arenaDimension && raceDaggerTileZ >= 0 && raceDaggerTileZ < arenaDimension)
			return arenaTiles[raceDaggerTileX, raceDaggerTileZ];

		return null;
	}

	public static int WorldToTileCoordinate(int arenaDimension, float worldCoordinate)
	{
		int arenaMiddle = arenaDimension / 2;
		return (int)MathF.Round(worldCoordinate / 4) + arenaMiddle;
	}

	public static int TileToWorldCoordinate(int arenaDimension, float tileCoordinate)
	{
		int arenaMiddle = arenaDimension / 2;
		return (int)MathF.Round((tileCoordinate - arenaMiddle) * 4);
	}

	public static float GetShrinkEndTime(float shrinkStart, float shrinkEnd, float shrinkRate)
	{
		shrinkStart = Math.Max(shrinkStart, 0);
		shrinkEnd = Math.Max(shrinkEnd, 0);

		if (shrinkRate <= 0 || shrinkEnd > shrinkStart)
			return 0;

		return (shrinkStart - shrinkEnd) / shrinkRate;
	}

	public static float GetShrinkTimeForTile(int arenaDimension, float shrinkStart, float shrinkEnd, float shrinkRate, int x, int y)
	{
		const int tileUnit = 4;
		float shrinkStartInTiles = shrinkStart / tileUnit;
		float shrinkEndInTiles = shrinkEnd / tileUnit;

		int arenaMiddle = arenaDimension / 2;
		Vector2 middle = new(arenaMiddle, arenaMiddle);
		float distance = Vector2.Distance(new(x, y), middle);
		if (distance > Math.Max(shrinkStartInTiles, shrinkEndInTiles))
			return 0;

		if (distance <= shrinkEndInTiles)
			return float.MaxValue;

		// TODO: Prevent this from overflowing.
		return (shrinkStartInTiles - distance) / shrinkRate * tileUnit;
	}

	public static float GetActualTileHeight(int arenaDimension, ImmutableArena arenaTiles, float shrinkStart, float shrinkEnd, float shrinkRate, int x, int y, float currentTime)
	{
		if (x < 0 || x >= arenaDimension)
			throw new ArgumentOutOfRangeException(nameof(x));
		if (y < 0 || y >= arenaDimension)
			throw new ArgumentOutOfRangeException(nameof(y));

		float tileHeight = arenaTiles[x, y];
		float shrinkTime = GetShrinkTimeForTile(arenaDimension, shrinkStart, shrinkEnd, shrinkRate, x, y);
		float shrinkHeight = Math.Max(0, currentTime - shrinkTime) / 4;
		return tileHeight - shrinkHeight;
	}

	public static float? GetActualRaceDaggerHeight(int arenaDimension, ImmutableArena arenaTiles, float shrinkStart, float shrinkEnd, float shrinkRate, int raceDaggerTileX, int raceDaggerTileZ, float currentTime)
	{
		if (raceDaggerTileX >= 0 && raceDaggerTileX < arenaDimension && raceDaggerTileZ >= 0 && raceDaggerTileZ < arenaDimension)
			return GetActualTileHeight(arenaDimension, arenaTiles, shrinkStart, shrinkEnd, shrinkRate, raceDaggerTileX, raceDaggerTileZ, currentTime);

		return null;
	}
}
