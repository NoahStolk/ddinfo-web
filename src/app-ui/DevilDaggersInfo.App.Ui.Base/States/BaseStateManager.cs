using DevilDaggersInfo.App.Ui.Base.States.Actions;

namespace DevilDaggersInfo.App.Ui.Base.States;

public static class BaseStateManager
{
	public static void Subscribe<TAction>(Action<TAction> eventHandler)
		where TAction : class, IAction<TAction>
	{
		TAction.Subscribe(eventHandler);
	}

	public static void Dispatch<TAction>(TAction action)
		where TAction : class, IAction<TAction>
	{
		// Dispatch an action, if it already exists for this action type, overwrite it.
		TAction.ActionToReduce = action;
	}

	public static void ReduceAll()
	{
		// TODO: Don't do this manually.
		Reduce<SetLayout>();
		Reduce<ValidateInstallation>();

		static void Reduce<T>()
			where T : class, IAction<T>
		{
			if (T.ActionToReduce == null)
				return;

			T.ActionToReduce.Reduce();
			foreach (Action<T> a in T.EventHandlers)
				a.Invoke(T.ActionToReduce);

			T.ActionToReduce = null;
		}
	}
}
