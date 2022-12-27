using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Extensions;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaLineState : IArenaState
{
	private Vector2i<int>? _lineStart;
	private float[,]? _newArena;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_lineStart = mousePosition.Real;
			_newArena = StateManager.SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			if (!_lineStart.HasValue || _newArena == null)
				return;

			Loop(mousePosition, (i, j) => _newArena[i, j] = StateManager.ArenaEditorState.SelectedHeight);

			Components.SpawnsetArena.Arena.UpdateArena(_newArena, SpawnsetEditType.ArenaLine);

			Reset();
		}
	}

	public void Reset()
	{
		_lineStart = null;
		_newArena = null;
	}

	public void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth)
	{
		Loop(mousePosition, (i, j) => Root.Game.RectangleRenderer.Schedule(new(Components.SpawnsetArena.Arena.TileSize), origin + new Vector2i<int>(i, j) * Components.SpawnsetArena.Arena.TileSize + Components.SpawnsetArena.Arena.HalfTile, depth, Color.HalfTransparentWhite));
	}

	private void Loop(ArenaMousePosition mousePosition, Action<int, int> action)
	{
		if (!_lineStart.HasValue)
			return;

		Vector2i<int> lineEnd = mousePosition.Real;
		PixelBounds rectangle = ArenaEditingUtils.GetRectangle(_lineStart.Value / Components.SpawnsetArena.Arena.TileSize, lineEnd / Components.SpawnsetArena.Arena.TileSize);
		for (int i = rectangle.X1; i <= rectangle.X2; i++)
		{
			for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
			{
				Vector2 visualTileCenter = new Vector2(i, j) * Components.SpawnsetArena.Arena.TileSize + Components.SpawnsetArena.Arena.HalfTile.ToVector2();
				if (ArenaEditingUtils.LineIntersectsSquare(_lineStart.Value.ToVector2(), lineEnd.ToVector2(), visualTileCenter, Components.SpawnsetArena.Arena.TileSize))
					action(i, j);
			}
		}
	}
}
