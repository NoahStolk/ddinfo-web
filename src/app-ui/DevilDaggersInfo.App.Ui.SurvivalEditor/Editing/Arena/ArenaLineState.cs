using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Extensions;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaLineState : IArenaState
{
	private readonly int _tileSize;

	private Vector2i<int>? _lineStart;

	public ArenaLineState(int tileSize)
	{
		_tileSize = tileSize;
	}

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_lineStart = mousePosition.Real;
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			Loop(mousePosition, (i, j) => Components.SpawnsetArena.Arena.UpdateArena(i, j, StateManager.ArenaEditorState.SelectedHeight));
			_lineStart = null;
			SpawnsetHistoryManager.Save(SpawnsetEditType.ArenaLine);
		}
	}

	public void Reset()
	{
		_lineStart = null;
	}

	public void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth)
	{
		Loop(mousePosition, (i, j) => Root.Game.RectangleRenderer.Schedule(new(_tileSize), origin + new Vector2i<int>(i, j) * _tileSize, depth, Color.HalfTransparentWhite));
	}

	private void Loop(ArenaMousePosition mousePosition, Action<int, int> action)
	{
		if (!_lineStart.HasValue)
			return;

		Vector2i<int> lineEnd = mousePosition.Real;
		Rectangle rectangle = ArenaEditingUtils.GetRectangle(_lineStart.Value / _tileSize, lineEnd / _tileSize);
		for (int i = rectangle.X1; i <= rectangle.X2; i++)
		{
			for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
			{
				Vector2 visualTileCenter = new Vector2(i, j) * _tileSize + new Vector2(_tileSize / 2f);
				if (ArenaEditingUtils.LineIntersectsSquare(_lineStart.Value.ToVector2(), lineEnd.ToVector2(), visualTileCenter, _tileSize))
					action(i, j);
			}
		}
	}
}
