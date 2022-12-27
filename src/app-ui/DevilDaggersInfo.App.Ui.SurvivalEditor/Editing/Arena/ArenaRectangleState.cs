using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaRectangleState : IArenaState
{
	private Vector2i<int>? _rectangleStart;
	private float[,]? _newArena;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_rectangleStart = mousePosition.Tile;
			_newArena = StateManager.SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			if (!_rectangleStart.HasValue || _newArena == null)
				return;

			Loop(mousePosition, (i, j) => _newArena[i, j] = StateManager.ArenaEditorState.SelectedHeight);

			Components.SpawnsetArena.Arena.UpdateArena(_newArena, SpawnsetEditType.ArenaRectangle);

			Reset();
		}
	}

	public void Reset()
	{
		_rectangleStart = null;
		_newArena = null;
	}

	public void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth)
	{
		Loop(mousePosition, (i, j) => Root.Game.RectangleRenderer.Schedule(new(Components.SpawnsetArena.Arena.TileSize), origin + new Vector2i<int>(i, j) * Components.SpawnsetArena.Arena.TileSize + Components.SpawnsetArena.Arena.HalfTile, depth, Color.HalfTransparentWhite));
	}

	private void Loop(ArenaMousePosition mousePosition, Action<int, int> action)
	{
		if (!_rectangleStart.HasValue)
			return;

		Vector2i<int> rectangleEnd = mousePosition.Tile;
		PixelBounds rectangle = ArenaEditingUtils.GetRectangle(_rectangleStart.Value, rectangleEnd);
		for (int i = rectangle.X1; i <= rectangle.X2; i++)
		{
			for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
				action(i, j);
		}
	}
}
