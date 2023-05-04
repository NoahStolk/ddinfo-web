using DevilDaggersInfo.App.Ui.SurvivalEditor.EditorArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.EditorArena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using System.Diagnostics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class Arena : AbstractComponent
{
	public const int TileSize = 6;

	private readonly ArenaPencilState _pencilState;
	private readonly ArenaLineState _lineState;
	private readonly ArenaRectangleState _rectangleState;
	private readonly ArenaEllipseState _ellipseState;
	private readonly ArenaBucketState _bucketState;
	private readonly ArenaDaggerState _daggerState;

	public Arena(IBounds bounds)
		: base(bounds)
	{
		_pencilState = new();
		_lineState = new();
		_rectangleState = new();
		_ellipseState = new();
		_bucketState = new();
		_daggerState = new();
	}

	public static Vector2i<int> HalfTile { get; } = new(TileSize / 2);
	public static Vector2 HalfTileAsVector2 { get; } = new(TileSize / 2f);

	private IArenaState GetActiveState() => StateManager.ArenaEditorState.ArenaTool switch
	{
		ArenaTool.Pencil => _pencilState,
		ArenaTool.Line => _lineState,
		ArenaTool.Rectangle => _rectangleState,
		ArenaTool.Ellipse => _ellipseState,
		ArenaTool.Bucket => _bucketState,
		ArenaTool.Dagger => _daggerState,
		_ => throw new UnreachableException(),
	};

	private ArenaMousePosition GetArenaMousePosition(Vector2i<int> scrollOffset)
	{
		int realX = (int)MouseUiContext.MousePosition.X - Bounds.X1 - scrollOffset.X;
		int realY = (int)MouseUiContext.MousePosition.Y - Bounds.Y1 - scrollOffset.Y;
		return new()
		{
			Real = new(realX, realY),
			Tile = new(realX / TileSize, realY / TileSize),
		};
	}

	private static void UpdateArena(int x, int y, float height, SpawnsetEditType spawnsetEditType)
	{
		float[,] newArena = StateManager.SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		newArena[x, y] = height;
		UpdateArena(newArena, spawnsetEditType);
	}

	public static void UpdateArena(float[,] newArena, SpawnsetEditType spawnsetEditType)
	{
		StateManager.Dispatch(new UpdateArena(newArena, spawnsetEditType));
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		ArenaMousePosition mousePosition = GetArenaMousePosition(scrollOffset);
		bool isOutOfRange = mousePosition.Tile.X < 0 || mousePosition.Tile.Y < 0 || mousePosition.Tile.X >= StateManager.SpawnsetState.Spawnset.ArenaDimension || mousePosition.Tile.Y >= StateManager.SpawnsetState.Spawnset.ArenaDimension;
		IArenaState activeState = GetActiveState();
		if (isOutOfRange)
		{
			activeState.HandleOutOfRange(mousePosition);
		}
		else
		{
			Root.Game.TooltipContext = new()
			{
				Text = $"{StateManager.SpawnsetState.Spawnset.ArenaTiles[mousePosition.Tile.X, mousePosition.Tile.Y]}\n{{{mousePosition.Tile.X}, {mousePosition.Tile.Y}}}",
				ForegroundColor = Color.HalfTransparentWhite,
				BackgroundColor = Color.HalfTransparentBlack,
			};
			int scroll = Input.GetScroll();
			if (scroll != 0)
			{
				// TODO: Selection.
				UpdateArena(mousePosition.Tile.X, mousePosition.Tile.Y, StateManager.SpawnsetState.Spawnset.ArenaTiles[mousePosition.Tile.X, mousePosition.Tile.Y] - scroll, SpawnsetEditType.ArenaTileHeight);
			}

			activeState.Handle(mousePosition);
		}
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		ScissorScheduler.PushScissor(Scissor.Create(Bounds, scrollOffset, ViewportState.Offset, ViewportState.Scale));

		IArenaState activeState = GetActiveState();
		activeState.Render(GetArenaMousePosition(scrollOffset), origin, Depth + 3);

		ScissorScheduler.PopScissor();
	}
}
