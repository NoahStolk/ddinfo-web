using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Silk.NET.GLFW;
using Warp;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaRectangleState : IArenaState
{
	private readonly int _tileSize;

	private Vector2i<int>? _rectangleStart;

	public ArenaRectangleState(int tileSize)
	{
		_tileSize = tileSize;
	}

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_rectangleStart = mousePosition.Tile;
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			Loop(mousePosition, (i, j) => Components.SpawnsetArena.Arena.UpdateArena(i, j, StateManager.ArenaEditorState.SelectedHeight));

			SpawnsetHistoryManager.Save(SpawnsetEditType.ArenaRectangle);
			_rectangleStart = null;
		}
	}

	public void Reset()
	{
		_rectangleStart = null;
	}

	public void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth)
	{
		Loop(mousePosition, (i, j) => RenderBatchCollector.RenderRectangleTopLeft(new(_tileSize), origin + new Vector2i<int>(i, j) * _tileSize, depth, GlobalColors.HalfTransparentWhite));
	}

	private void Loop(ArenaMousePosition mousePosition, Action<int, int> action)
	{
		if (!_rectangleStart.HasValue)
			return;

		Vector2i<int> rectangleEnd = mousePosition.Tile;
		Rectangle rectangle = ArenaEditingUtils.GetRectangle(_rectangleStart.Value, rectangleEnd);
		for (int i = rectangle.X1; i <= rectangle.X2; i++)
		{
			for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
				action(i, j);
		}
	}
}
