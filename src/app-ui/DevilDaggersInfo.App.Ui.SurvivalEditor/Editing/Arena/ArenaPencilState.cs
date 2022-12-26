using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Extensions;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaPencilState : IArenaState
{
	private Vector2i<int>? _pencilStart;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_pencilStart = mousePosition.Real;
		}
		else if (Input.IsButtonHeld(MouseButton.Left))
		{
			if (!_pencilStart.HasValue)
				return;

			Vector2i<int> pencilEnd = mousePosition.Real;
			PixelBounds rectangle = ArenaEditingUtils.GetRectangle(_pencilStart.Value / Components.SpawnsetArena.Arena.TileSize, pencilEnd / Components.SpawnsetArena.Arena.TileSize);
			for (int i = rectangle.X1; i <= rectangle.X2; i++)
			{
				for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
				{
					Vector2 visualTileCenter = new Vector2(i, j) * Components.SpawnsetArena.Arena.TileSize + Components.SpawnsetArena.Arena.HalfTile.ToVector2();
					if (ArenaEditingUtils.LineIntersectsSquare(_pencilStart.Value.ToVector2(), pencilEnd.ToVector2(), visualTileCenter, Components.SpawnsetArena.Arena.TileSize))
						Components.SpawnsetArena.Arena.UpdateArena(i, j, StateManager.ArenaEditorState.SelectedHeight);
				}
			}

			Components.SpawnsetArena.Arena.UpdateArena(mousePosition.Tile.X, mousePosition.Tile.Y, StateManager.ArenaEditorState.SelectedHeight);
			_pencilStart = mousePosition.Real;
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			_pencilStart = null;
			StateManager.Dispatch(new SaveHistory(SpawnsetEditType.ArenaPencil));
		}
	}

	public void Reset()
	{
		_pencilStart = null;
	}

	public void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth)
	{
	}
}
