using DevilDaggersInfo.App.Ui.SpawnsetEditor.State;
using ImGuiNET;
using Silk.NET.Maths;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SpawnsetEditor.Arena;

public readonly record struct ArenaMousePosition(Vector2 Real, Vector2D<int> Tile, bool IsValid)
{
	// TODO: Fix offset not being entirely accurate (you can hover over the part next to the top left corner of the canvas, and clicking the very end at the bottom right doesn't activate the controls).
	public static ArenaMousePosition Get(ImGuiIOPtr io, Vector2 offset)
	{
		float realX = io.MousePos.X - offset.X;
		float realY = io.MousePos.Y - offset.Y;
		Vector2 real = new(realX, realY);
		Vector2D<int> tile = new((int)(real.X / ArenaChild.TileSize), (int)(real.Y / ArenaChild.TileSize));
		bool isValid = tile is { X: >= 0, Y: >= 0 } && tile.X < SpawnsetState.Spawnset.ArenaDimension && tile.Y < SpawnsetState.Spawnset.ArenaDimension;

		return new()
		{
			Real = real,
			Tile = tile,
			IsValid = isValid,
		};
	}
}
