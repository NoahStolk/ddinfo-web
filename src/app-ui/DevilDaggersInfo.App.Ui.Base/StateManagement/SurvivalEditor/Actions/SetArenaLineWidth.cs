namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetArenaLineWidth(float Width) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.ArenaLineState = new(Width);
	}
}
