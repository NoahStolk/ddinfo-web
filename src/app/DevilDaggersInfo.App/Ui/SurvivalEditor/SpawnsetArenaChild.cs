using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SpawnsetArenaChild
{
	public const int TileSize = 6;

	private static readonly Vector2 _arenaSize = new(TileSize * SpawnsetBinary.ArenaDimensionMax);

	private static float _currentSecond;

	public static void Render()
	{
		ImGui.BeginChild("ArenaChild", new(400 - 8, 768 - 64));

		ImGui.BeginChild("Arena", _arenaSize);

		RenderArena();

		ImGuiIOPtr io = ImGui.GetIO();
		ArenaMousePosition mousePosition = ArenaMousePosition.Get(io, ImGui.GetWindowPos());

		if (mousePosition.IsValid)
		{
			ImGui.SetTooltip($"{SpawnsetState.Spawnset.ArenaTiles[(int)mousePosition.Tile.X, (int)mousePosition.Tile.Y]}\n{mousePosition.Tile.ToString("0")}");
			if (io.MouseWheel != 0)
			{
				float[,] newTiles = SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
				newTiles[(int)mousePosition.Tile.X, (int)mousePosition.Tile.Y] -= io.MouseWheel;
				SpawnsetState.Spawnset = SpawnsetState.Spawnset with
				{
					ArenaTiles = new(SpawnsetState.Spawnset.ArenaDimension, newTiles),
				};
				SpawnsetHistoryUtils.Save(SpawnsetEditType.ArenaTileHeight);
			}
		}

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
			int arenaMiddle = SpawnsetState.Spawnset.ArenaDimension / 2;
			float realRaceX = SpawnsetState.Spawnset.RaceDaggerPosition.X / 4f + arenaMiddle;
			float realRaceZ = SpawnsetState.Spawnset.RaceDaggerPosition.Y / 4f + arenaMiddle;

			const int halfSize = TileSize / 2;

			(int raceX, _, int raceZ) = SpawnsetState.Spawnset.GetRaceDaggerTilePosition();
			float actualHeight = SpawnsetState.Spawnset.GetActualTileHeight(raceX, raceZ, _currentSecond);
			Color tileColor = TileUtils.GetColorFromHeight(actualHeight);
			Color invertedTileColor = Color.Invert(tileColor);
			Vector3 daggerColor = Vector3.Lerp(invertedTileColor, invertedTileColor.Intensify(96), MathF.Sin((float)ImGui.GetTime()) / 2 + 0.5f);

			Vector2 center = origin + new Vector2(realRaceX * TileSize + halfSize, realRaceZ * TileSize + halfSize);
			drawList.AddCircle(center, 3, ImGui.GetColorU32(Color.FromVector3(daggerColor)));
			// Root.Game.SpriteRenderer.Schedule(new(-8, -8), origin.ToVector2() + new Vector2(realRaceX * TileSize + halfSize, realRaceZ * TileSize + halfSize), Depth + 3, ContentManager.Content.IconDaggerTexture, Color.FromVector3(daggerColor));
		}

		const int tileUnit = 4;
		Vector2 arenaCenter = origin + new Vector2((int)(SpawnsetBinary.ArenaDimensionMax / 2f * TileSize));

		float shrinkStartRadius = SpawnsetState.Spawnset.ShrinkStart / tileUnit * TileSize;
		if (shrinkStartRadius > 0)
			drawList.AddCircle(arenaCenter, shrinkStartRadius, ImGui.GetColorU32(Color.Blue));

		float shrinkEndTime = SpawnsetState.Spawnset.GetShrinkEndTime();
		float shrinkRadius = shrinkEndTime == 0 ? SpawnsetState.Spawnset.ShrinkStart : Math.Max(SpawnsetState.Spawnset.ShrinkStart - _currentSecond / shrinkEndTime * (SpawnsetState.Spawnset.ShrinkStart - SpawnsetState.Spawnset.ShrinkEnd), SpawnsetState.Spawnset.ShrinkEnd);
		float shrinkCurrentRadius = shrinkRadius / tileUnit * TileSize;
		if (shrinkCurrentRadius > 0)
			drawList.AddCircle(arenaCenter, shrinkCurrentRadius, ImGui.GetColorU32(Color.Purple));

		float shrinkEndRadius = SpawnsetState.Spawnset.ShrinkEnd / tileUnit * TileSize;
		if (shrinkEndRadius > 0)
			drawList.AddCircle(arenaCenter, shrinkEndRadius, ImGui.GetColorU32(Color.Red));
	}

	private readonly record struct ArenaMousePosition(Vector2 Real, Vector2 Tile, bool IsValid)
	{
		public static ArenaMousePosition Get(ImGuiIOPtr io, Vector2 offset)
		{
			int realX = (int)(io.MousePos.X - offset.X);
			int realY = (int)(io.MousePos.Y - offset.Y);
			Vector2 real = new(realX, realY);
			Vector2 tile = new(MathF.Floor(real.X / TileSize), MathF.Floor(real.Y / TileSize));
			bool isValid = tile is { X: >= 0, Y: >= 0 } && tile.X < SpawnsetState.Spawnset.ArenaDimension && tile.Y < SpawnsetState.Spawnset.ArenaDimension;

			return new()
			{
				Real = real,
				Tile = tile,
				IsValid = isValid,
			};
		}
	}
}
