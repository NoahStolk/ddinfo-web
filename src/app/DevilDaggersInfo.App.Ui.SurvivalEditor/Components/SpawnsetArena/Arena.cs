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

	public override void Update(Vector2i<int> scrollOffset)
	{
		IArenaState activeState = GetActiveState();
		if (isOutOfRange)
		{
			activeState.HandleOutOfRange(mousePosition);
		}
		else
		{
			activeState.Handle(mousePosition);
		}
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		IArenaState activeState = GetActiveState();
		activeState.Render(GetArenaMousePosition(scrollOffset), origin, Depth + 3);
	}
}
