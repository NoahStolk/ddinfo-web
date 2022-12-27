namespace DevilDaggersInfo.App.Ui.Base.StateManagement;

/// <summary>
/// Represents an action dispatched by the state manager. There can only be one action of a specific type at a time. If multiple actions are dispatched during the same update, the last one will be the one that is executed.
/// </summary>
public interface IAction
{
	/// <summary>
	/// Modifies state.
	/// </summary>
	void Reduce(StateReducer stateReducer);
}
