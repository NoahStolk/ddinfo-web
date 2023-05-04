using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Arena;

public readonly record struct ArenaMousePosition(Vector2 Real, Vector2i<int> Tile, bool IsValid)
{
	public static ArenaMousePosition Get(ImGuiIOPtr io, Vector2 offset)
	{
		int realX = (int)(io.MousePos.X - offset.X);
		int realY = (int)(io.MousePos.Y - offset.Y);
		Vector2 real = new(realX, realY);
		Vector2i<int> tile = new((int)real.X / ArenaChild.TileSize, (int)real.Y / ArenaChild.TileSize);
		bool isValid = tile is { X: >= 0, Y: >= 0 } && tile.X < SpawnsetState.Spawnset.ArenaDimension && tile.Y < SpawnsetState.Spawnset.ArenaDimension;

		return new()
		{
			Real = real,
			Tile = tile,
			IsValid = isValid,
		};
	}
}
