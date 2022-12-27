using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateRaceDaggerPosition(Vector2 NewRaceDaggerPosition) : IAction
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				RaceDaggerPosition = NewRaceDaggerPosition,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.RaceDagger);
	}
}
