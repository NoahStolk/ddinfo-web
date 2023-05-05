using DevilDaggersInfo.App.Ui.SurvivalEditor.Arena.EditorControls;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.App.Utils;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using System.Diagnostics;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Arena;

public static class ArenaChild
{
	public const int TileSize = 8;
	public const int HalfTileSize = TileSize / 2;
	public static readonly Vector2 HalfTileSizeAsVector2 = new(HalfTileSize);

	public static readonly Vector2 ArenaSize = new(TileSize * SpawnsetBinary.ArenaDimensionMax);

	private static readonly ArenaPencilState _pencilState = new();
	private static readonly ArenaLineState _lineState = new();
	private static readonly ArenaRectangleState _rectangleState = new();
	private static readonly ArenaEllipseState _ellipseState = new();
	private static readonly ArenaBucketState _bucketState = new();
	private static readonly ArenaDaggerState _daggerState = new();

	private static float _currentSecond;

	public static ImGuiIoState LeftMouse { get; } = new(true, 0);

	public static float CurrentSecond => _currentSecond;

	public static float SelectedHeight { get; set; }
	public static ArenaTool ArenaTool { get; set; }

	private static IArenaState GetActiveState() => ArenaTool switch
	{
		 ArenaTool.Pencil => _pencilState,
		 ArenaTool.Line => _lineState,
		 ArenaTool.Rectangle => _rectangleState,
		 ArenaTool.Ellipse => _ellipseState,
		 ArenaTool.Bucket => _bucketState,
		 ArenaTool.Dagger => _daggerState,
		 _ => throw new UnreachableException(),
	};

	public static void Render()
	{
		ImGui.BeginChild("ArenaChild", new(400 - 8, 768 - 64));

		ImGui.BeginChild("Arena", ArenaSize);

		// Update
		ImGuiIOPtr io = ImGui.GetIO();
		LeftMouse.Update(io);

		ArenaMousePosition mousePosition = ArenaMousePosition.Get(io, ImGui.GetWindowPos());

		if (mousePosition.IsValid)
		{
			ImGui.SetTooltip($"{SpawnsetState.Spawnset.ArenaTiles[mousePosition.Tile.X, mousePosition.Tile.Y]}\n{mousePosition.Tile.ToString("0")}");
			if (io.MouseWheel != 0)
			{
				float[,] newTiles = SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
				newTiles[mousePosition.Tile.X, mousePosition.Tile.Y] -= io.MouseWheel;
				SpawnsetState.Spawnset = SpawnsetState.Spawnset with { ArenaTiles = new(SpawnsetState.Spawnset.ArenaDimension, newTiles) };
				SpawnsetHistoryUtils.Save(SpawnsetEditType.ArenaTileHeight);
			}
		}

		IArenaState activeState = GetActiveState();
		if (mousePosition.IsValid)
			activeState.Handle(mousePosition);
		else
			activeState.HandleOutOfRange(mousePosition);

		// Render
		ArenaCanvas.Render();
		activeState.Render(mousePosition);

		ImGui.EndChild();

		ImGui.SliderFloat("Time", ref _currentSecond, 0, SpawnsetState.Spawnset.GetSliderMaxSeconds());
		ArenaEditorControls.Render();
		ArenaHeightButtons.Render();

		ImGui.EndChild();
	}
}
