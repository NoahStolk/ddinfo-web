using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Arena.EditorStates;

public class ArenaPencilState : IArenaState
{
	private Vector2? _pencilStart;
	private HashSet<Vector2i<int>>? _modifiedCoords;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (ArenaChild.LeftMouse.JustPressed)
		{
			_pencilStart = mousePosition.Real;
			_modifiedCoords = new();
		}
		else if (ArenaChild.LeftMouse.Down)
		{
			if (!_pencilStart.HasValue || _modifiedCoords == null)
				return;

			Vector2 pencilEnd = mousePosition.Real;
			Vector2 start = ArenaEditingUtils.Snap(_pencilStart.Value, ArenaChild.TileSize) + ArenaChild.HalfTileSizeAsVector2;
			Vector2 end = ArenaEditingUtils.Snap(pencilEnd, ArenaChild.TileSize) + ArenaChild.HalfTileSizeAsVector2;
			ArenaEditingUtils.Stadium stadium = new(start, end, StateManager.ArenaPencilState.Size / 2 * ArenaChild.TileSize);
			for (int i = 0; i < SpawnsetBinary.ArenaDimensionMax; i++)
			{
				for (int j = 0; j < SpawnsetBinary.ArenaDimensionMax; j++)
				{
					Vector2i<int> target = new(i, j);
					if (_modifiedCoords.Contains(target)) // Early rejection, even though we're using a HashSet.
						continue;

					Vector2 visualTileCenter = new Vector2(i, j) * ArenaChild.TileSize + ArenaChild.HalfTileSizeAsVector2;

					ArenaEditingUtils.Square square = ArenaEditingUtils.Square.FromCenter(visualTileCenter, ArenaChild.TileSize);
					if (square.IntersectsStadium(stadium))
						_modifiedCoords.Add(target);
				}
			}

			_modifiedCoords.Add(new(mousePosition.Tile.X, mousePosition.Tile.Y));
			_pencilStart = mousePosition.Real;
		}
		else if (ArenaChild.LeftMouse.JustReleased)
		{
			Emit();
		}
	}

	public void HandleOutOfRange(ArenaMousePosition mousePosition)
	{
		if (ArenaChild.LeftMouse.Down)
			_pencilStart = mousePosition.Real;
		else if (ArenaChild.LeftMouse.JustReleased)
			Emit();
	}

	private void Emit()
	{
		if (!_pencilStart.HasValue || _modifiedCoords == null)
			return;

		float[,] newArena = SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();

		foreach (Vector2i<int> position in _modifiedCoords)
			newArena[position.X, position.Y] = ArenaChild.SelectedHeight;

		SpawnsetState.Spawnset = SpawnsetState.Spawnset with { ArenaTiles = new(SpawnsetState.Spawnset.ArenaDimension, newArena) };
		SpawnsetHistoryUtils.Save(SpawnsetEditType.ArenaPencil);

		Reset();
	}

	private void Reset()
	{
		_pencilStart = null;
		_modifiedCoords = null;
	}

	public void Render(ArenaMousePosition mousePosition)
	{
		ImDrawListPtr drawList = ImGui.GetWindowDrawList();

		Vector2 origin = ImGui.GetCursorScreenPos();
		drawList.AddCircleFilled(origin + GetSnappedPosition(mousePosition.Real), GetDisplayRadius(), ImGui.GetColorU32(_pencilStart.HasValue ? Color.White : Color.HalfTransparentWhite));

		if (_modifiedCoords == null)
			return;

		for (int i = 0; i < SpawnsetBinary.ArenaDimensionMax; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimensionMax; j++)
			{
				if (_modifiedCoords.Contains(new(i, j)))
				{
					Vector2 topLeft = origin + new Vector2(i, j) * ArenaChild.TileSize;
					drawList.AddRectFilled(topLeft, topLeft + new Vector2(ArenaChild.TileSize), ImGui.GetColorU32(Color.HalfTransparentWhite));
				}
			}
		}
	}

	private static float GetDisplayRadius()
	{
		return StateManager.ArenaPencilState.Size / 2 * ArenaChild.TileSize;
	}

	private static Vector2 GetSnappedPosition(Vector2 position)
	{
		return ArenaEditingUtils.Snap(position, ArenaChild.TileSize) + ArenaChild.HalfTileSizeAsVector2;
	}
}
