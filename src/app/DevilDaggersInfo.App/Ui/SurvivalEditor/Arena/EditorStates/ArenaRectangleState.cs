using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Arena.EditorStates;

public class ArenaRectangleState : IArenaState
{
	private Vector2i<int>? _rectangleStart;
	private float[,]? _newArena;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (ArenaChild.LeftMouse.JustPressed)
		{
			_rectangleStart = mousePosition.Tile;
			_newArena = SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		}
		else if (ArenaChild.LeftMouse.JustReleased)
		{
			Emit(mousePosition);
		}
	}

	public void HandleOutOfRange(ArenaMousePosition mousePosition)
	{
		if (ArenaChild.LeftMouse.JustReleased)
			Emit(mousePosition);
	}

	private void Emit(ArenaMousePosition mousePosition)
	{
		if (!_rectangleStart.HasValue || _newArena == null)
			return;

		Loop(mousePosition, (i, j) => _newArena[i, j] = ArenaChild.SelectedHeight);

		SpawnsetState.Spawnset = SpawnsetState.Spawnset with { ArenaTiles = new(SpawnsetState.Spawnset.ArenaDimension, _newArena) };
		SpawnsetHistoryUtils.Save(SpawnsetEditType.ArenaRectangle);

		Reset();
	}

	private void Reset()
	{
		_rectangleStart = null;
		_newArena = null;
	}

	public void Render(ArenaMousePosition mousePosition)
	{
		ImDrawListPtr drawList = ImGui.GetWindowDrawList();

		Loop(mousePosition, (i, j) =>
		{
			Vector2 origin = ImGui.GetCursorScreenPos();
			Vector2 min = origin + new Vector2(i, j) * ArenaChild.TileSize;
			drawList.AddRectFilled(min, min + new Vector2(ArenaChild.TileSize), ImGui.GetColorU32(Color.HalfTransparentWhite));
		});
	}

	private void Loop(ArenaMousePosition mousePosition, Action<int, int> action)
	{
		if (!_rectangleStart.HasValue)
			return;

		ArenaEditingUtils.Rectangle rectangle = ArenaEditingUtils.GetRectangle(_rectangleStart.Value, mousePosition.Tile);

		int startXa = Math.Max(0, rectangle.X1);
		int startYa = Math.Max(0, rectangle.Y1);
		int endXa = Math.Min(SpawnsetState.Spawnset.ArenaDimension - 1, rectangle.X2);
		int endYa = Math.Min(SpawnsetState.Spawnset.ArenaDimension - 1, rectangle.Y2);

		if (StateManager.ArenaRectangleState.Filled)
		{
			for (int i = startXa; i <= endXa; i++)
			{
				for (int j = startYa; j <= endYa; j++)
					action(i, j);
			}
		}
		else
		{
			int addedSize = StateManager.ArenaRectangleState.Thickness;
			int startXb = Math.Max(0, rectangle.X1 + addedSize);
			int startYb = Math.Max(0, rectangle.Y1 + addedSize);
			int endXb = Math.Min(SpawnsetState.Spawnset.ArenaDimension - 1, rectangle.X2 - addedSize);
			int endYb = Math.Min(SpawnsetState.Spawnset.ArenaDimension - 1, rectangle.Y2 - addedSize);

			for (int i = startXa; i <= endXa; i++)
			{
				for (int j = startYa; j <= endYa; j++)
				{
					if (i >= startXb && i <= endXb && j >= startYb && j <= endYb)
						continue;

					action(i, j);
				}
			}
		}
	}
}