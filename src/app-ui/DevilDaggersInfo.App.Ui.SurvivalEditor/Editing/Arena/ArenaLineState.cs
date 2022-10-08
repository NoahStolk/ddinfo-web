using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Silk.NET.GLFW;
using Warp;
using Warp.Extensions;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaLineState : IArenaState
{
	private readonly int _tileSize;

	private Vector2i<int>? _lineStart;

	public ArenaLineState(int tileSize)
	{
		_tileSize = tileSize;
	}

	public void Handle(int relMouseX, int relMouseY, int x, int y)
	{
		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_lineStart = new(relMouseX, relMouseY);
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			if (_lineStart.HasValue)
			{
				Vector2i<int> lineEnd = new(relMouseX, relMouseY);
				Rectangle rectangle = ArenaEditingUtils.GetRectangle(_lineStart.Value / _tileSize, lineEnd / _tileSize);
				for (int i = rectangle.X1; i <= rectangle.X2; i++)
				{
					for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
					{
						Vector2 visualTileCenter = new Vector2(i, j) * _tileSize + new Vector2(_tileSize / 2f);
						if (ArenaEditingUtils.LineIntersectsSquare(_lineStart.Value.ToVector2(), lineEnd.ToVector2(), visualTileCenter, _tileSize))
							Components.SpawnsetArena.Arena.UpdateArena(i, j, StateManager.ArenaEditorState.SelectedHeight);
					}
				}

				Components.SpawnsetArena.Arena.UpdateArena(x, y, StateManager.ArenaEditorState.SelectedHeight);
			}

			_lineStart = null;
			SpawnsetHistoryManager.Save(SpawnsetEditType.ArenaLine);
		}
	}

	public void Reset()
	{
		_lineStart = null;
	}
}
