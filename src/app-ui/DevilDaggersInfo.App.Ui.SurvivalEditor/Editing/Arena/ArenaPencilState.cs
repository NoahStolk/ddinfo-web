using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Extensions;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaPencilState : IArenaState
{
	private Vector2i<int>? _pencilStart;
	private List<Vector2i<byte>>? _modifiedCoords;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_pencilStart = mousePosition.Real;
			_modifiedCoords = new();
		}
		else if (Input.IsButtonHeld(MouseButton.Left))
		{
			if (!_pencilStart.HasValue || _modifiedCoords == null)
				return;

			Vector2i<int> pencilEnd = mousePosition.Real;
			PixelBounds rectangle = ArenaEditingUtils.GetRectangle(_pencilStart.Value / Components.SpawnsetArena.Arena.TileSize, pencilEnd / Components.SpawnsetArena.Arena.TileSize);
			for (int i = rectangle.X1; i <= rectangle.X2; i++)
			{
				for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
				{
					Vector2i<byte> target = new((byte)i, (byte)j);
					if (_modifiedCoords.Contains(target))
						continue;

					Vector2 visualTileCenter = new Vector2(i, j) * Components.SpawnsetArena.Arena.TileSize + Components.SpawnsetArena.Arena.HalfTile.ToVector2();
					if (ArenaEditingUtils.LineIntersectsSquare(_pencilStart.Value.ToVector2(), pencilEnd.ToVector2(), visualTileCenter, Components.SpawnsetArena.Arena.TileSize))
						_modifiedCoords.Add(target);
				}
			}

			Vector2i<byte> finalTarget = new((byte)mousePosition.Tile.X, (byte)mousePosition.Tile.Y);
			if (!_modifiedCoords.Contains(finalTarget))
				_modifiedCoords.Add(finalTarget);
			_pencilStart = mousePosition.Real;
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			if (!_pencilStart.HasValue || _modifiedCoords == null)
				return;

			float[,] newArena = StateManager.SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();

			foreach (Vector2i<byte> position in _modifiedCoords)
				newArena[position.X, position.Y] = StateManager.ArenaEditorState.SelectedHeight;

			Components.SpawnsetArena.Arena.UpdateArena(newArena, SpawnsetEditType.ArenaPencil);

			Reset();
		}
	}

	public void Reset()
	{
		_pencilStart = null;
		_modifiedCoords = null;
	}

	public void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth)
	{
		if (_modifiedCoords == null)
			return;

		for (int i = 0; i < SpawnsetBinary.ArenaDimensionMax; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimensionMax; j++)
			{
				Vector2i<byte> target = new((byte)i, (byte)j);
				if (_modifiedCoords.Contains(target))
					Root.Game.RectangleRenderer.Schedule(new(Components.SpawnsetArena.Arena.TileSize), origin + new Vector2i<int>(i, j) * Components.SpawnsetArena.Arena.TileSize + Components.SpawnsetArena.Arena.HalfTile, depth, Color.HalfTransparentWhite);
			}
		}
	}
}
