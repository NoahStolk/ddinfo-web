using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateTimerStart(float TimerStart) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				TimerStart = TimerStart,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.TimerStart);
	}
}
