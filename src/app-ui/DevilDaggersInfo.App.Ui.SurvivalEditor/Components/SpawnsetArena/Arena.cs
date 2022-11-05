using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Warp;
using Warp.Extensions;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class Arena : AbstractComponent
{
	private readonly int _tileSize;

	private readonly ArenaPencilState _pencilState;
	private readonly ArenaLineState _lineState;
	private readonly ArenaRectangleState _rectangleState;
	private readonly ArenaBucketState _bucketState;
	private readonly ArenaDaggerState _daggerState;

	private readonly List<IArenaState> _states;

	private float _currentSecond;
	private float _shrinkRadius;

	public Arena(Vector2i<int> topLeft, int tileSize)
		: base(new(topLeft.X, topLeft.Y, topLeft.X + tileSize * SpawnsetBinary.ArenaDimensionMax, topLeft.Y + tileSize * SpawnsetBinary.ArenaDimensionMax))
	{
		_tileSize = tileSize;

		_pencilState = new(_tileSize);
		_lineState = new(_tileSize);
		_rectangleState = new();
		_bucketState = new();
		_daggerState = new();

		_states = new()
		{
			_pencilState,
			_lineState,
			_rectangleState,
			_bucketState,
			_daggerState,
		};
	}

	private IArenaState GetActiveState() => StateManager.ArenaEditorState.ArenaTool switch
	{
		ArenaTool.Pencil => _pencilState,
		ArenaTool.Line => _lineState,
		ArenaTool.Rectangle => _rectangleState,
		ArenaTool.Bucket => _bucketState,
		ArenaTool.Dagger => _daggerState,
		_ => throw new InvalidEnumConversionException(StateManager.ArenaEditorState.ArenaTool),
	};

	private ArenaMousePosition GetArenaMousePosition(Vector2i<int> parentPosition)
	{
		int realX = (int)MouseUiContext.MousePosition.X - Metric.X1 - parentPosition.X;
		int realY = (int)MouseUiContext.MousePosition.Y - Metric.Y1 - parentPosition.Y;
		return new()
		{
			Real = new(realX, realY),
			Tile = new(realX / _tileSize, realY / _tileSize),
		};
	}

	public static void UpdateArena(int x, int y, float height)
	{
		float[,] newArena = StateManager.SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		newArena[x, y] = height;
		UpdateArena(newArena);
	}

	public static void UpdateArena(float[,] newArena)
	{
		StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with
		{
			ArenaTiles = new(StateManager.SpawnsetState.Spawnset.ArenaDimension, newArena),
		});
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		bool hover = MouseUiContext.Contains(parentPosition, Metric);
		if (!hover)
		{
			Reset();
			return;
		}

		ArenaMousePosition mousePosition = GetArenaMousePosition(parentPosition);
		if (mousePosition.Tile.X < 0 || mousePosition.Tile.Y < 0 || mousePosition.Tile.X >= StateManager.SpawnsetState.Spawnset.ArenaDimension || mousePosition.Tile.Y >= StateManager.SpawnsetState.Spawnset.ArenaDimension)
		{
			Reset();
			return;
		}

		Root.Game.TooltipText = $"{StateManager.SpawnsetState.Spawnset.ArenaTiles[mousePosition.Tile.X, mousePosition.Tile.Y]}\n{{{mousePosition.Tile.X}, {mousePosition.Tile.Y}}}";
		int scroll = Input.GetScroll();
		if (scroll != 0)
		{
			// TODO: Selection.
			UpdateArena(mousePosition.Tile.X, mousePosition.Tile.Y, StateManager.SpawnsetState.Spawnset.ArenaTiles[mousePosition.Tile.X, mousePosition.Tile.Y] + scroll);
			SpawnsetHistoryManager.Save(SpawnsetEditType.ArenaTileHeight);
			return;
		}

		IArenaState activeState = GetActiveState();
		activeState.Handle(mousePosition);
	}

	private void Reset()
	{
		foreach (IArenaState state in _states)
			state.Reset();
	}

	public void SetShrinkCurrent(float currentSecond)
	{
		_currentSecond = currentSecond;

		SpawnsetBinary spawnset = StateManager.SpawnsetState.Spawnset;
		float shrinkEndTime = spawnset.GetShrinkEndTime();
		_shrinkRadius = shrinkEndTime == 0 ? spawnset.ShrinkStart : Math.Max(spawnset.ShrinkStart - _currentSecond / shrinkEndTime * (spawnset.ShrinkStart - spawnset.ShrinkEnd), spawnset.ShrinkEnd);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> origin = parentPosition + new Vector2i<int>(Metric.X1, Metric.Y1);
		Vector2i<int> center = origin + new Vector2i<int>((int)(SpawnsetBinary.ArenaDimensionMax / 2f * _tileSize));
		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size, origin, Depth, Color.Black);

		for (int i = 0; i < StateManager.SpawnsetState.Spawnset.ArenaDimension; i++)
		{
			for (int j = 0; j < StateManager.SpawnsetState.Spawnset.ArenaDimension; j++)
			{
				int x = i * _tileSize;
				int y = j * _tileSize;

				float actualHeight = StateManager.SpawnsetState.Spawnset.GetActualTileHeight(i, j, _currentSecond);
				float height = StateManager.SpawnsetState.Spawnset.ArenaTiles[i, j];
				Color colorCurrent = TileUtils.GetColorFromHeight(actualHeight);
				Color colorValue = TileUtils.GetColorFromHeight(height);
				if (Math.Abs(actualHeight - height) < 0.001f)
				{
					if (Color.Black != colorValue)
						RenderBatchCollector.RenderRectangleTopLeft(new(_tileSize), origin + new Vector2i<int>(x, y), Depth + 1, colorValue);
				}
				else
				{
					if (Color.Black != colorCurrent)
						RenderBatchCollector.RenderRectangleTopLeft(new(_tileSize), origin + new Vector2i<int>(x, y), Depth + 1, colorCurrent);

					if (Color.Black != colorValue)
					{
						const int size = 2;
						const int padding = 2;
						RenderBatchCollector.RenderRectangleTopLeft(new(size), origin + new Vector2i<int>(x + padding, y + padding), Depth + 2, colorValue);
					}
				}
			}
		}

		if (StateManager.SpawnsetState.Spawnset.GameMode == GameMode.Race)
		{
			(int raceX, _, int raceZ) = StateManager.SpawnsetState.Spawnset.GetRaceDaggerTilePosition();
			int arenaMiddle = StateManager.SpawnsetState.Spawnset.ArenaDimension / 2;
			float realRaceX = StateManager.SpawnsetState.Spawnset.RaceDaggerPosition.X / 4f + arenaMiddle;
			float realRaceZ = StateManager.SpawnsetState.Spawnset.RaceDaggerPosition.Y / 4f + arenaMiddle;

			int halfSize = _tileSize / 2;

			float actualHeight = StateManager.SpawnsetState.Spawnset.GetActualTileHeight(raceX, raceZ, _currentSecond);
			float lerp = MathF.Sin(Root.Game.Tt) / 2 + 0.5f;
			Color tileColor = TileUtils.GetColorFromHeight(actualHeight);
			Color inverted = Color.Invert(tileColor);
			Vector3 color = Vector3.Lerp(inverted, inverted.Intensify(96), lerp);
			RenderBatchCollector.RenderSprite(new(-8, -8), origin.ToVector2() + new Vector2(realRaceX * _tileSize + halfSize, realRaceZ * _tileSize + halfSize), Depth + 3, ContentManager.Content.IconDaggerTexture, Color.FromVector3(color));
		}

		RenderBatchCollector.SetScissor(Scissor.FromComponent(Metric, parentPosition));

		const int tileUnit = 4;
		float shrinkStartRadius = StateManager.SpawnsetState.Spawnset.ShrinkStart / tileUnit * _tileSize;
		float shrinkCurrentRadius = _shrinkRadius / tileUnit * _tileSize;
		float shrinkEndRadius = StateManager.SpawnsetState.Spawnset.ShrinkEnd / tileUnit * _tileSize;

		if (shrinkStartRadius > 0)
			RenderBatchCollector.RenderCircleCenter(center, shrinkStartRadius, Depth + 5, Color.Purple);
		if (shrinkCurrentRadius > 0)
			RenderBatchCollector.RenderCircleCenter(center, shrinkCurrentRadius, Depth + 4, Color.Yellow);
		if (shrinkEndRadius > 0)
			RenderBatchCollector.RenderCircleCenter(center, shrinkEndRadius, Depth + 5, Color.Aqua);

		RenderBatchCollector.UnsetScissor();

		IArenaState activeState = GetActiveState();
		activeState.Render(GetArenaMousePosition(parentPosition), origin, Depth + 3);
	}
}
