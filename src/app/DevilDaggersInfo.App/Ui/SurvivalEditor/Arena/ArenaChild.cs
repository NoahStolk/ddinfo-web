using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Arena.EditorControls;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using System.Diagnostics;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Arena;

public static class ArenaChild
{
	public const int TileSize = 8;
	public const int HalfTileSize = TileSize / 2;

	private static readonly Vector2 _arenaSize = new(TileSize * SpawnsetBinary.ArenaDimensionMax);

	// private static readonly ArenaPencilState _pencilState = new();
	// private static readonly ArenaLineState _lineState = new();
	private static readonly ArenaRectangleState _rectangleState = new();
	// private static readonly ArenaEllipseState _ellipseState = new();
	// private static readonly ArenaBucketState _bucketState = new();
	// private static readonly ArenaDaggerState _daggerState = new();

	private static float _currentSecond;
	private static bool _leftMouseDownPrevious;

	public static bool LeftMouseJustPressed { get; private set; }
	public static bool LeftMouseJustReleased { get; private set; }

	public static float SelectedHeight { get; set; }
	public static ArenaTool ArenaTool { get; set; }

	private static IArenaState GetActiveState() => ArenaTool switch
	{
		 // ArenaTool.Pencil => _pencilState,
		 // ArenaTool.Line => _lineState,
		 ArenaTool.Rectangle => _rectangleState,
		 // ArenaTool.Ellipse => _ellipseState,
		 // ArenaTool.Bucket => _bucketState,
		 // ArenaTool.Dagger => _daggerState,
		 _ => _rectangleState,//throw new UnreachableException(),
	};

	public static void Render()
	{
		ImGui.BeginChild("ArenaChild", new(400 - 8, 768 - 64));

		ImGui.BeginChild("Arena", _arenaSize);

		// Update
		ImGuiIOPtr io = ImGui.GetIO();
		bool leftMouseDown = io.MouseDown[0];
		LeftMouseJustPressed = leftMouseDown && !_leftMouseDownPrevious;
		LeftMouseJustReleased = !leftMouseDown && _leftMouseDownPrevious;
		_leftMouseDownPrevious = leftMouseDown;

		ArenaMousePosition mousePosition = ArenaMousePosition.Get(io, ImGui.GetWindowPos());

		if (mousePosition.IsValid)
		{
			ImGui.SetTooltip($"{SpawnsetState.Spawnset.ArenaTiles[mousePosition.Tile.X, mousePosition.Tile.Y]}\n{mousePosition.Tile.ToString("0")}");
			if (io.MouseWheel != 0)
			{
				float[,] newTiles = SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
				newTiles[mousePosition.Tile.X, mousePosition.Tile.Y] -= io.MouseWheel;
				SpawnsetState.Spawnset = SpawnsetState.Spawnset with
				{
					ArenaTiles = new(SpawnsetState.Spawnset.ArenaDimension, newTiles),
				};
				SpawnsetHistoryUtils.Save(SpawnsetEditType.ArenaTileHeight);
			}
		}

		IArenaState activeState = GetActiveState();
		if (mousePosition.IsValid)
			activeState.Handle(mousePosition);
		else
			activeState.HandleOutOfRange(mousePosition);

		// Render
		RenderArena();

		activeState.Render(mousePosition);

		ImGui.EndChild();

		ImGui.SliderFloat("Time", ref _currentSecond, 0, SpawnsetState.Spawnset.GetSliderMaxSeconds());
		ArenaEditorControls.Render();

		ImGui.EndChild();
	}

	private static void RenderArena()
	{
		ImDrawListPtr drawList = ImGui.GetWindowDrawList();

		Vector2 origin = ImGui.GetCursorScreenPos();
		drawList.AddRectFilled(origin, origin + _arenaSize, ImGui.GetColorU32(new Vector4(0, 0, 0, 1)));

		for (int i = 0; i < SpawnsetState.Spawnset.ArenaDimension; i++)
		{
			for (int j = 0; j < SpawnsetState.Spawnset.ArenaDimension; j++)
			{
				int x = i * TileSize;
				int y = j * TileSize;

				float actualHeight = SpawnsetState.Spawnset.GetActualTileHeight(i, j, _currentSecond);
				float height = SpawnsetState.Spawnset.ArenaTiles[i, j];
				Color colorCurrent = TileUtils.GetColorFromHeight(actualHeight);
				Color colorValue = TileUtils.GetColorFromHeight(height);
				Vector2 min = origin + new Vector2(x, y);

				if (Math.Abs(actualHeight - height) < 0.001f)
				{
					if (Color.Black != colorValue)
					{
						drawList.AddRectFilled(min, min + new Vector2(TileSize), ImGui.GetColorU32(colorValue));
					}
				}
				else
				{
					if (Color.Black != colorCurrent)
					{
						drawList.AddRectFilled(min, min + new Vector2(TileSize), ImGui.GetColorU32(colorCurrent));
					}

					if (Color.Black != colorValue)
					{
						const int offset = 2;
						const int size = 2;
						drawList.AddRectFilled(min + new Vector2(offset), min + new Vector2(offset) + new Vector2(size), ImGui.GetColorU32(colorValue));
					}
				}
			}
		}

		if (SpawnsetState.Spawnset.GameMode == GameMode.Race)
		{
			int arenaMiddle = SpawnsetState.Spawnset.ArenaDimension / 2;
			float realRaceX = SpawnsetState.Spawnset.RaceDaggerPosition.X / 4f + arenaMiddle;
			float realRaceZ = SpawnsetState.Spawnset.RaceDaggerPosition.Y / 4f + arenaMiddle;

			const int halfSize = TileSize / 2;

			(int raceX, _, int raceZ) = SpawnsetState.Spawnset.GetRaceDaggerTilePosition();
			float actualHeight = SpawnsetState.Spawnset.GetActualTileHeight(raceX, raceZ, _currentSecond);
			Color tileColor = TileUtils.GetColorFromHeight(actualHeight);
			Color invertedTileColor = Color.Invert(tileColor);
			Vector3 daggerColor = Vector3.Lerp(invertedTileColor, invertedTileColor.Intensify(96), MathF.Sin((float)ImGui.GetTime()) / 2 + 0.5f);

			Vector2 center = origin + new Vector2(realRaceX * TileSize + halfSize, realRaceZ * TileSize + halfSize);
			drawList.AddCircle(center, 3, ImGui.GetColorU32(Color.FromVector3(daggerColor)));
			// Root.Game.SpriteRenderer.Schedule(new(-8, -8), origin.ToVector2() + new Vector2(realRaceX * TileSize + halfSize, realRaceZ * TileSize + halfSize), Depth + 3, ContentManager.Content.IconDaggerTexture, Color.FromVector3(daggerColor));
		}

		const int tileUnit = 4;
		Vector2 arenaCenter = origin + new Vector2((int)(SpawnsetBinary.ArenaDimensionMax / 2f * TileSize));

		float shrinkStartRadius = SpawnsetState.Spawnset.ShrinkStart / tileUnit * TileSize;
		if (shrinkStartRadius > 0)
			drawList.AddCircle(arenaCenter, shrinkStartRadius, ImGui.GetColorU32(Color.Blue));

		float shrinkEndTime = SpawnsetState.Spawnset.GetShrinkEndTime();
		float shrinkRadius = shrinkEndTime == 0 ? SpawnsetState.Spawnset.ShrinkStart : Math.Max(SpawnsetState.Spawnset.ShrinkStart - _currentSecond / shrinkEndTime * (SpawnsetState.Spawnset.ShrinkStart - SpawnsetState.Spawnset.ShrinkEnd), SpawnsetState.Spawnset.ShrinkEnd);
		float shrinkCurrentRadius = shrinkRadius / tileUnit * TileSize;
		if (shrinkCurrentRadius > 0)
			drawList.AddCircle(arenaCenter, shrinkCurrentRadius, ImGui.GetColorU32(Color.Purple));

		float shrinkEndRadius = SpawnsetState.Spawnset.ShrinkEnd / tileUnit * TileSize;
		if (shrinkEndRadius > 0)
			drawList.AddCircle(arenaCenter, shrinkEndRadius, ImGui.GetColorU32(Color.Red));
	}
}
