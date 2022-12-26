namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateArena(float[,] NewArena) : IAction<UpdateArena>
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				ArenaTiles = new(StateManager.SpawnsetState.Spawnset.ArenaDimension, NewArena),
			},
		};
	}
}
