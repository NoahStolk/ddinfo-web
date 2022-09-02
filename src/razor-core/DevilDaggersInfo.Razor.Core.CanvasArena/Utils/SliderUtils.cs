using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.Razor.Core.CanvasArena.Utils;

public static class SliderUtils
{
	public static float GetSliderMaxSeconds(SpawnsetBinary spawnsetBinary)
	{
		// Determine the max tile height to add additional time to the slider.
		// For example, when the shrink ends at 200, but there is a tile at height 20, we want to add another 88 seconds ((20 + 2) * 4) to the slider so the shrink transition is always fully visible for all tiles.
		// Add 2 heights to make sure it is still visible until the height is -2 (the palette should still show something until a height of at least -1 or lower).
		// Multiply by 4 because a tile falls by 1 unit every 4 seconds.
		float maxTileHeight = 0;
		for (int i = 0; i < spawnsetBinary.ArenaDimension; i++)
		{
			for (int j = 0; j < spawnsetBinary.ArenaDimension; j++)
			{
				float tileHeight = spawnsetBinary.ArenaTiles[i, j];
				if (maxTileHeight < tileHeight)
					maxTileHeight = tileHeight;
			}
		}

		return spawnsetBinary.GetShrinkEndTime() + (maxTileHeight + 2) * 4;
	}
}
