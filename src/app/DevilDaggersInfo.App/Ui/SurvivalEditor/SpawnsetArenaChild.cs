using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SpawnsetArenaChild
{
	public const int TileSize = 6;

	private static readonly Vector2 _arenaSize = new(TileSize * SpawnsetBinary.ArenaDimensionMax);

	private static float _currentSecond;
	private static float _shrinkRadius;

	public static void Render()
	{
		ImGui.BeginChild("ArenaChild", new(400 - 8, 768 - 64));

		ImGui.BeginChild("Arena", _arenaSize);

		RenderArena();

		ImGui.EndChild();

		ImGui.SliderFloat("Time", ref _currentSecond, 0, SpawnsetState.Spawnset.GetSliderMaxSeconds());

		ImGui.EndChild();
	}

	private static void RenderArena()
	{
		ImDrawListPtr drawList = ImGui.GetWindowDrawList();

		Vector2 origin = ImGui.GetCursorScreenPos();
		drawList.AddRectFilled(origin, origin + _arenaSize, ImGui.GetColorU32(new Vector4(0, 0, 0, 1)));

		for (int i = 0; i < SpawnsetState.Spawnset.ArenaDimension; i++)
		{
			for (int j = 0; j < SpawnsetState.Spawnset.ArenaDimension; j++)
			{
				int x = i * TileSize;
				int y = j * TileSize;

				float actualHeight = SpawnsetState.Spawnset.GetActualTileHeight(i, j, _currentSecond);
				float height = SpawnsetState.Spawnset.ArenaTiles[i, j];
				Color colorCurrent = TileUtils.GetColorFromHeight(actualHeight);
				Color colorValue = TileUtils.GetColorFromHeight(height);
				Vector2 min = origin + new Vector2(x, y);

				if (Math.Abs(actualHeight - height) < 0.001f)
				{
					if (Color.Black != colorValue)
					{
						drawList.AddRectFilled(min, min + new Vector2(TileSize), ImGui.GetColorU32(colorValue));
					}
				}
				else
				{
					if (Color.Black != colorCurrent)
					{
						drawList.AddRectFilled(min, min + new Vector2(TileSize), ImGui.GetColorU32(colorCurrent));
					}

					if (Color.Black != colorValue)
					{
						const int offset = 2;
						const int size = 2;
						drawList.AddRectFilled(min + new Vector2(offset), min + new Vector2(offset) + new Vector2(size), ImGui.GetColorU32(colorValue));
					}
				}
			}
		}

		if (SpawnsetState.Spawnset.GameMode == GameMode.Race)
		{
			int arenaMiddle = StateManager.SpawnsetState.Spawnset.ArenaDimension / 2;
			float realRaceX = StateManager.SpawnsetState.Spawnset.RaceDaggerPosition.X / 4f + arenaMiddle;
			float realRaceZ = StateManager.SpawnsetState.Spawnset.RaceDaggerPosition.Y / 4f + arenaMiddle;

			const int halfSize = TileSize / 2;

			(int raceX, _, int raceZ) = StateManager.SpawnsetState.Spawnset.GetRaceDaggerTilePosition();
			float actualHeight = StateManager.SpawnsetState.Spawnset.GetActualTileHeight(raceX, raceZ, _currentSecond);
			Color tileColor = TileUtils.GetColorFromHeight(actualHeight);
			Color invertedTileColor = Color.Invert(tileColor);
			Vector3 daggerColor = Vector3.Lerp(invertedTileColor, invertedTileColor.Intensify(96), MathF.Sin((float)ImGui.GetTime()) / 2 + 0.5f);

			Vector2 center = origin + new Vector2(realRaceX * TileSize + halfSize, realRaceZ * TileSize + halfSize);
			drawList.AddCircle(center, 6, ImGui.GetColorU32(Color.FromVector3(daggerColor)));
			// Root.Game.SpriteRenderer.Schedule(new(-8, -8), origin.ToVector2() + new Vector2(realRaceX * TileSize + halfSize, realRaceZ * TileSize + halfSize), Depth + 3, ContentManager.Content.IconDaggerTexture, Color.FromVector3(daggerColor));
		}
	}
}
