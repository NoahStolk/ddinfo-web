namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateRaceDaggerPosition(Vector2 NewRaceDaggerPosition) : IAction<UpdateRaceDaggerPosition>
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
	}
}
