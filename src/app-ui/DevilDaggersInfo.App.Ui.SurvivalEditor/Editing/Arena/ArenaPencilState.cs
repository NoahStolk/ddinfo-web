using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Silk.NET.GLFW;
using Warp;
using Warp.Extensions;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaPencilState : IArenaState
{
	private readonly int _tileSize;

	private Vector2i<int>? _pencilStart;

	public ArenaPencilState(int tileSize)
	{
		_tileSize = tileSize;
	}

	public void Handle(int relMouseX, int relMouseY, int x, int y)
	{
		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_pencilStart = new(relMouseX, relMouseY);
		}
		else if (Input.IsButtonHeld(MouseButton.Left))
		{
			if (!_pencilStart.HasValue)
				return;

			Vector2i<int> pencilEnd = new(relMouseX, relMouseY);
			Rectangle rectangle = ArenaEditingUtils.GetRectangle(_pencilStart.Value / _tileSize, pencilEnd / _tileSize);
			for (int i = rectangle.X1; i <= rectangle.X2; i++)
			{
				for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
				{
					Vector2 visualTileCenter = new Vector2(i, j) * _tileSize + new Vector2(_tileSize / 2f);
					if (ArenaEditingUtils.LineIntersectsSquare(_pencilStart.Value.ToVector2(), pencilEnd.ToVector2(), visualTileCenter, _tileSize))
						Components.SpawnsetArena.Arena.UpdateArena(i, j, StateManager.ArenaEditorState.SelectedHeight);
				}
			}

			Components.SpawnsetArena.Arena.UpdateArena(x, y, StateManager.ArenaEditorState.SelectedHeight);
			_pencilStart = new(relMouseX, relMouseY);
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			_pencilStart = null;
			SpawnsetHistoryManager.Save(SpawnsetEditType.ArenaPencil);
		}
	}

	public void Reset()
	{
		_pencilStart = null;
	}
}
