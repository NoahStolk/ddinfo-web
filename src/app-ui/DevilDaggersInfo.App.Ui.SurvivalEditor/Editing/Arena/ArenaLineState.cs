using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Extensions;

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
		Vector2 start = ArenaEditingUtils.Snap(_lineStart.Value.ToVector2(), Vector2.One);
		Vector2 end = ArenaEditingUtils.Snap(lineEnd.ToVector2(), Vector2.One);
		ArenaEditingUtils.Stadium stadium = new(start, end, StateManager.ArenaLineState.Width / 2 * Components.SpawnsetArena.Arena.TileSize);
		for (int i = 0; i < SpawnsetBinary.ArenaDimensionMax; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimensionMax; j++)
			{
				Vector2 visualTileCenter = new Vector2(i, j) * Components.SpawnsetArena.Arena.TileSize + Components.SpawnsetArena.Arena.HalfTile.ToVector2();

				ArenaEditingUtils.Square square = ArenaEditingUtils.Square.FromCenter(visualTileCenter, Components.SpawnsetArena.Arena.TileSize);
				if (square.IntersectsStadium(stadium))
					action(i, j);
			}
		}
	}
}
