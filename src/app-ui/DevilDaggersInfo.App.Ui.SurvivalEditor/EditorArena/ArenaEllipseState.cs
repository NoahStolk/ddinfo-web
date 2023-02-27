using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.EditorArena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
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
			_ellipseStart = mousePosition.Real;
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

		ArenaEditingUtils.AlignedEllipse? ellipse = GetEllipse(mousePosition);
		if (ellipse.HasValue)
			Root.Game.CircleRenderer.Schedule(origin + ellipse.Value.Center.RoundToVector2Int32(), ellipse.Value.Radius, depth + 1, Color.White);
	}

	private void Loop(ArenaMousePosition mousePosition, Action<int, int> action)
	{
		ArenaEditingUtils.AlignedEllipse? ellipse = GetEllipse(mousePosition);
		if (!ellipse.HasValue)
			return;

		for (int i = 0; i < StateManager.SpawnsetState.Spawnset.ArenaDimension; i++)
		{
			for (int j = 0; j < StateManager.SpawnsetState.Spawnset.ArenaDimension; j++)
			{
				Vector2 visualTileCenter = new Vector2(i, j) * Arena.TileSize + Arena.HalfTileAsVector2;

				ArenaEditingUtils.Square square = ArenaEditingUtils.Square.FromCenter(visualTileCenter, Arena.TileSize);
				if (ellipse.Value.Contains(square))
					action(i, j);
			}
		}
	}

	private ArenaEditingUtils.AlignedEllipse? GetEllipse(ArenaMousePosition mousePosition)
	{
		if (!_ellipseStart.HasValue)
			return null;

		return GetEllipse(GetSnappedPosition(_ellipseStart.Value).ToVector2(), GetSnappedPosition(mousePosition.Real).ToVector2());
	}

	private static ArenaEditingUtils.AlignedEllipse GetEllipse(Vector2 a, Vector2 b)
	{
		Vector2 center = (a + b) * 0.5f;
		return new(center, center - b);
	}

	private static Vector2i<int> GetSnappedPosition(Vector2i<int> position)
	{
		return ArenaEditingUtils.Snap(position, Arena.TileSize) + Arena.HalfTile;
	}
}
