using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Silk.NET.GLFW;
using Warp;
using Warp.Extensions;
using Warp.Ui;
using Warp.Ui.Components;
using Warp.Utils;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class Arena : AbstractComponent
{
	private readonly int _tileSize;

	private Vector2i<int>? _pencilStart;
	private Vector2i<int>? _lineStart;
	private Vector2i<int>? _rectangleStart;

	private float _currentSecond;
	private float _shrinkRadius;

	public Arena(Vector2i<int> topLeft, int tileSize)
		: base(new(topLeft.X, topLeft.Y, topLeft.X + tileSize * SpawnsetBinary.ArenaDimensionMax, topLeft.Y + tileSize * SpawnsetBinary.ArenaDimensionMax))
	{
		_tileSize = tileSize;
	}

	private static void UpdateArena(int x, int y, float height)
	{
		float[,] newArena = StateManager.SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		newArena[x, y] = height;
		UpdateArena(newArena);
	}

	private static void UpdateArena(float[,] newArena)
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

		int relMouseX = (int)MouseUiContext.MousePosition.X - Metric.X1 - parentPosition.X;
		int relMouseY = (int)MouseUiContext.MousePosition.Y - Metric.Y1 - parentPosition.Y;
		int x = relMouseX / _tileSize;
		int y = relMouseY / _tileSize;
		if (x < 0 || y < 0 || x >= StateManager.SpawnsetState.Spawnset.ArenaDimension || y >= StateManager.SpawnsetState.Spawnset.ArenaDimension)
		{
			Reset();
			return;
		}

		Root.Game.TooltipText = $"{StateManager.SpawnsetState.Spawnset.ArenaTiles[x, y]}\n{{{x}, {y}}}";
		int scroll = Input.GetScroll();
		if (scroll != 0)
		{
			// TODO: Selection.
			UpdateArena(x, y, StateManager.SpawnsetState.Spawnset.ArenaTiles[x, y] + scroll);
			return;
		}

		switch (StateManager.ArenaEditorState.ArenaTool)
		{
			case ArenaTool.Pencil:
				if (Input.IsButtonPressed(MouseButton.Left))
				{
					_pencilStart = new(relMouseX, relMouseY);
				}
				else if (Input.IsButtonHeld(MouseButton.Left))
				{
					if (_pencilStart.HasValue)
					{
						Vector2i<int> pencilEnd = new(relMouseX, relMouseY);
						Rectangle rectangle = GetRectangle(_pencilStart.Value / _tileSize, pencilEnd / _tileSize);
						for (int i = rectangle.X1; i <= rectangle.X2; i++)
						{
							for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
							{
								Vector2 visualTileCenter = new Vector2(i, j) * _tileSize + new Vector2(_tileSize / 2f);
								if (LineIntersectsSquare(_pencilStart.Value.ToVector2(), pencilEnd.ToVector2(), visualTileCenter, _tileSize))
									UpdateArena(i, j, StateManager.ArenaEditorState.SelectedHeight);
							}
						}

						UpdateArena(x, y, StateManager.ArenaEditorState.SelectedHeight);
						_pencilStart = new(relMouseX, relMouseY);
					}
				}
				else if (Input.IsButtonReleased(MouseButton.Left))
				{
					_pencilStart = null;
					SpawnsetHistoryManager.Save(SpawnsetEditType.ArenaPencil);
				}

				break;
			case ArenaTool.Line:
				if (Input.IsButtonPressed(MouseButton.Left))
				{
					_lineStart = new(relMouseX, relMouseY);
				}
				else if (Input.IsButtonReleased(MouseButton.Left))
				{
					if (_lineStart.HasValue)
					{
						Vector2i<int> lineEnd = new(relMouseX, relMouseY);
						Rectangle rectangle = GetRectangle(_lineStart.Value / _tileSize, lineEnd / _tileSize);
						for (int i = rectangle.X1; i <= rectangle.X2; i++)
						{
							for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
							{
								Vector2 visualTileCenter = new Vector2(i, j) * _tileSize + new Vector2(_tileSize / 2f);
								if (LineIntersectsSquare(_lineStart.Value.ToVector2(), lineEnd.ToVector2(), visualTileCenter, _tileSize))
									UpdateArena(i, j, StateManager.ArenaEditorState.SelectedHeight);
							}
						}

						UpdateArena(x, y, StateManager.ArenaEditorState.SelectedHeight);
					}

					_lineStart = null;
					SpawnsetHistoryManager.Save(SpawnsetEditType.ArenaLine);
				}

				break;
			case ArenaTool.Rectangle:
				if (Input.IsButtonPressed(MouseButton.Left))
				{
					_rectangleStart = new(x, y);
				}
				else if (Input.IsButtonReleased(MouseButton.Left))
				{
					if (_rectangleStart.HasValue)
					{
						Vector2i<int> rectangleEnd = new(x, y);
						Rectangle rectangle = GetRectangle(_rectangleStart.Value, rectangleEnd);
						for (int i = rectangle.X1; i <= rectangle.X2; i++)
						{
							for (int j = rectangle.Y1; j <= rectangle.Y2; j++)
								UpdateArena(i, j, StateManager.ArenaEditorState.SelectedHeight);
						}
					}

					SpawnsetHistoryManager.Save(SpawnsetEditType.ArenaRectangle);
					_rectangleStart = null;
				}

				break;
			case ArenaTool.Bucket:
				if (Input.IsButtonPressed(MouseButton.Left))
				{
					List<Vector2i<int>> done = new();
					float[,] tiles = StateManager.SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
					int dimension = StateManager.SpawnsetState.Spawnset.ArenaDimension;
					float targetHeight = StateManager.SpawnsetState.Spawnset.ArenaTiles[x, y];
					FillNeighbors(x, y);

					UpdateArena(tiles);
					SpawnsetHistoryManager.Save(SpawnsetEditType.ArenaBucket);

					void FillNeighbors(int x, int y)
					{
						tiles[x, y] = StateManager.ArenaEditorState.SelectedHeight;
						done.Add(new(x, y));

						int leftX = x - 1;
						int rightX = x + 1;
						int topY = y - 1;
						int bottomY = y + 1;

						if (leftX >= 0)
							FillIfApplicable(leftX, y);
						if (rightX < dimension)
							FillIfApplicable(rightX, y);
						if (topY >= 0)
							FillIfApplicable(x, topY);
						if (bottomY < dimension)
							FillIfApplicable(x, bottomY);

						void FillIfApplicable(int newX, int newY)
						{
							const float tolerance = 0.1f; // TODO: Configurable.
							if (!done.Contains(new(newX, newY)) && MathF.Abs(tiles[newX, newY] - targetHeight) < tolerance)
								FillNeighbors(newX, newY);
						}
					}
				}

				break;
			case ArenaTool.Dagger:
				if (Input.IsButtonHeld(MouseButton.Left))
				{
					StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with
					{
						RaceDaggerPosition = new(StateManager.SpawnsetState.Spawnset.TileToWorldCoordinate(x), StateManager.SpawnsetState.Spawnset.TileToWorldCoordinate(y)),
					});
				}
				else if (Input.IsButtonReleased(MouseButton.Left))
				{
					SpawnsetHistoryManager.Save(SpawnsetEditType.RaceDagger);
				}

				break;
		}
	}

	private void Reset()
	{
		_pencilStart = null;
		_lineStart = null;
		_rectangleStart = null;
	}

	private static Rectangle GetRectangle(Vector2i<int> start, Vector2i<int> end)
	{
		bool startXMin = start.X < end.X;
		int minX, maxX;
		if (startXMin)
		{
			minX = start.X;
			maxX = end.X;
		}
		else
		{
			minX = end.X;
			maxX = start.X;
		}

		bool startYMin = start.Y < end.Y;
		int minY, maxY;
		if (startYMin)
		{
			minY = start.Y;
			maxY = end.Y;
		}
		else
		{
			minY = end.Y;
			maxY = start.Y;
		}

		return new(minX, minY, maxX, maxY);
	}

	private static bool LineIntersectsSquare(Vector2 lineA, Vector2 lineB, Vector2 squarePosition, float squareSize)
	{
		float squareHalfSize = squareSize / 2f;
		Vector2 squareTl = squarePosition + new Vector2(-squareHalfSize, -squareHalfSize);
		Vector2 squareTr = squarePosition + new Vector2(+squareHalfSize, -squareHalfSize);
		Vector2 squareBl = squarePosition + new Vector2(-squareHalfSize, +squareHalfSize);
		Vector2 squareBr = squarePosition + new Vector2(+squareHalfSize, +squareHalfSize);
		Vector2 lineNormal = GetLineNormal(lineA, lineB);

		// Calculate booleans for all 4 points; return true immediately when any of the booleans do not hold the same value.
		bool tlBehind = PointIsBehindPlane(lineA, lineNormal, squareTl);
		if (PointIsBehindPlane(lineA, lineNormal, squareTr) != tlBehind)
			return true;

		if (PointIsBehindPlane(lineA, lineNormal, squareBl) != tlBehind)
			return true;

		return PointIsBehindPlane(lineA, lineNormal, squareBr) != tlBehind;
	}

	private static Vector2 GetLineNormal(Vector2 lineA, Vector2 lineB)
	{
		float deltaX = lineB.X - lineA.X;
		float deltaY = lineB.Y - lineA.Y;
		return new(-deltaY, deltaX);
	}

	private static bool PointIsBehindPlane(Vector2 linePoint, Vector2 lineNormal, Vector2 point)
	{
		return Vector2.Dot(lineNormal, Vector2.Normalize(point - linePoint)) < 0;
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
					if (!ColorsEqual(Color.Black, colorValue))
						RenderBatchCollector.RenderRectangleTopLeft(new(_tileSize), origin + new Vector2i<int>(x, y), Depth + 1, colorValue);
				}
				else
				{
					if (!ColorsEqual(Color.Black, colorCurrent))
						RenderBatchCollector.RenderRectangleTopLeft(new(_tileSize), origin + new Vector2i<int>(x, y), Depth + 1, colorCurrent);

					if (!ColorsEqual(Color.Black, colorValue))
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
			float lerp = MathF.Sin(CoreBase.Game.Tt) / 2 + 0.5f;
			Color tileColor = TileUtils.GetColorFromHeight(actualHeight);
			Color inverted = Color.Invert(tileColor);
			Vector3 color = Lerp(inverted, inverted.Intensify(96), lerp);
			RenderBatchCollector.RenderSprite(new(8), origin.ToVector2() + new Vector2(realRaceX * _tileSize + halfSize, realRaceZ * _tileSize + halfSize), Depth + 3, ContentManager.Content.IconDaggerTexture, ToColor(color));
		}

		RenderBatchCollector.SetScissor(Scissor.FromComponent(Metric, parentPosition));

		const int tileUnit = 4;
		RenderBatchCollector.RenderCircleCenter(center, StateManager.SpawnsetState.Spawnset.ShrinkStart / tileUnit * _tileSize, Depth + 5, Color.Purple);
		RenderBatchCollector.RenderCircleCenter(center, _shrinkRadius / tileUnit * _tileSize, Depth + 4, Color.Yellow);
		RenderBatchCollector.RenderCircleCenter(center, StateManager.SpawnsetState.Spawnset.ShrinkEnd / tileUnit * _tileSize, Depth + 5, Color.Aqua);

		RenderBatchCollector.UnsetScissor();

		// TODO: Move to Warp.
		static Color ToColor(Vector3 vector)
		{
			return new((byte)(vector.X * byte.MaxValue), (byte)(vector.Y * byte.MaxValue), (byte)(vector.Z * byte.MaxValue), byte.MaxValue);
		}

		// TODO: Move to Warp.
		static bool ColorsEqual(Color a, Color b)
		{
			return a.R == b.R && a.G == b.G && a.B == b.B;
		}

		// TODO: Move to Warp.
		static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
		{
			return new(MathUtils.Lerp(value1.X, value2.X, amount), MathUtils.Lerp(value1.Y, value2.Y, amount), MathUtils.Lerp(value1.Z, value2.Z, amount));
		}
	}
}
