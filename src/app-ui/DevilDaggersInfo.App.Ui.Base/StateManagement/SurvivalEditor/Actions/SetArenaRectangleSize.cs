namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetArenaRectangleSize(int Size) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.ArenaRectangleState = stateReducer.ArenaRectangleState with
		{
			Size = Size,
		};
	}
}
