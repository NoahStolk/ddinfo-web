using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateShrinkRate(float ShrinkRate) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				ShrinkRate = ShrinkRate,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.ShrinkRate);
	}
}
