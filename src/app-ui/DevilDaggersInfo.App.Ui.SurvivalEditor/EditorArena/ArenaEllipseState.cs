using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.EditorArena.Data;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Extensions;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.EditorArena;

public class ArenaEllipseState : IArenaState
{
	private Vector2i<int>? _ellipseStart;
	private float[,]? _newArena;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_ellipseStart = mousePosition.Tile;
			_newArena = StateManager.SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			Emit(mousePosition);
		}
	}

	public void HandleOutOfRange(ArenaMousePosition mousePosition)
	{
		if (Input.IsButtonReleased(MouseButton.Left))
			Emit(mousePosition);
	}

	private void Emit(ArenaMousePosition mousePosition)
	{
		if (!_ellipseStart.HasValue || _newArena == null)
			return;

		Loop(mousePosition, (i, j) => _newArena[i, j] = StateManager.ArenaEditorState.SelectedHeight);

		Arena.UpdateArena(_newArena, SpawnsetEditType.ArenaEllipse);

		Reset();
	}

	private void Reset()
	{
		_ellipseStart = null;
		_newArena = null;
	}

	public void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth)
	{
		Loop(mousePosition, (i, j) => Root.Game.RectangleRenderer.Schedule(new(Arena.TileSize), origin + new Vector2i<int>(i, j) * Arena.TileSize + Arena.HalfTile, depth, Color.HalfTransparentWhite));
	}

	private void Loop(ArenaMousePosition mousePosition, Action<int, int> action)
	{
		if (!_ellipseStart.HasValue)
			return;

		float radius = Vector2.Distance(_ellipseStart.Value.ToVector2(), mousePosition.Tile.ToVector2()) / 2f;
		Vector2 center = Vector2.Lerp(_ellipseStart.Value.ToVector2(), mousePosition.Tile.ToVector2(), 0.5f);
		for (int i = 0; i < StateManager.SpawnsetState.Spawnset.ArenaDimension; i++)
		{
			for (int j = 0; j < StateManager.SpawnsetState.Spawnset.ArenaDimension; j++)
			{
				if (Vector2.Distance(new(i, j), center) <= radius)
					action(i, j);
			}
		}
	}
}
