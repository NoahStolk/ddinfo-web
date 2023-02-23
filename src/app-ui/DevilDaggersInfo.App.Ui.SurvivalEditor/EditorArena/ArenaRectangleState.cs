using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.EditorArena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.EditorArena;

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

			Arena.UpdateArena(_newArena, SpawnsetEditType.ArenaRectangle);

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
		Loop(mousePosition, (i, j) => Root.Game.RectangleRenderer.Schedule(new(Arena.TileSize), origin + new Vector2i<int>(i, j) * Arena.TileSize + Arena.HalfTile, depth, Color.HalfTransparentWhite));
	}

	private void Loop(ArenaMousePosition mousePosition, Action<int, int> action)
	{
		if (!_rectangleStart.HasValue)
			return;

		Vector2i<int> rectangleEnd = mousePosition.Tile;
		PixelBounds rectangle = ArenaEditingUtils.GetRectangle(_rectangleStart.Value, rectangleEnd);

		bool filled = StateManager.ArenaRectangleState.Filled;
		int addedSize = StateManager.ArenaRectangleState.Size - 1;
		int startXa = Math.Max(0, rectangle.X1 - addedSize);
		int startYa = Math.Max(0, rectangle.Y1 - addedSize);
		int endXa = Math.Min(StateManager.SpawnsetState.Spawnset.ArenaDimension - 1, rectangle.X2 + addedSize);
		int endYa = Math.Min(StateManager.SpawnsetState.Spawnset.ArenaDimension - 1, rectangle.Y2 + addedSize);

		if (filled)
		{
			for (int i = startXa; i <= endXa; i++)
			{
				for (int j = startYa; j <= endYa; j++)
					action(i, j);
			}
		}
		else
		{
			int startXb = Math.Max(0, rectangle.X1);
			int startYb = Math.Max(0, rectangle.Y1);
			int endXb = Math.Min(StateManager.SpawnsetState.Spawnset.ArenaDimension - 1, rectangle.X2);
			int endYb = Math.Min(StateManager.SpawnsetState.Spawnset.ArenaDimension - 1, rectangle.Y2);

			for (int i = startXa; i <= endXa; i++)
			{
				bool isEdgeX = i >= startXa && i <= startXb || i >= endXb && i <= endXa;
				for (int j = startYa; j <= endYa; j++)
				{
					bool isEdgeY = j >= startYa && j <= startYb || j >= endYb && j <= endYa;
					if (!isEdgeX && !isEdgeY)
						continue;

					action(i, j);
				}
			}
		}
	}
}
