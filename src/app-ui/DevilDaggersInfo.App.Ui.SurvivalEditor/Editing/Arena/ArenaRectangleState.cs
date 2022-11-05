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
	private Vector2i<int>? _rectangleStart;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_rectangleStart = mousePosition.Tile;
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			if (_rectangleStart.HasValue)
			{
				Vector2i<int> rectangleEnd = mousePosition.Tile;
				Rectangle rectangle = ArenaEditingUtils.GetRectangle(_rectangleStart.Value, rectangleEnd);
				for (int i = rectangle.X1; i <= rectangle.X2; i++)
				{
					for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
						Components.SpawnsetArena.Arena.UpdateArena(i, j, StateManager.ArenaEditorState.SelectedHeight);
				}
			}

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
	}
}
