namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

/// <summary>
/// Represents an action dispatched by the state manager. There can only be one action of a specific type at a time. If multiple actions are dispatched during the same update, the last one will be the one that is executed.
/// </summary>
/// <typeparam name="TSelf">The type that implements the interface.</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IAction<TSelf>
	where TSelf : class, IAction<TSelf>
{
	static virtual TSelf? ActionToReduce { get; set; }

	static virtual List<Action<TSelf>> EventHandlers { get; } = new();

	static virtual void Subscribe(Action<TSelf> action)
	{
		TSelf.EventHandlers.Add(action);
	}

	/// <summary>
	/// Modifies state.
	/// </summary>
	void Reduce();
}
