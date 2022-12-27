using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateArena(float[,] NewArena, SpawnsetEditType SpawnsetEditType) : IAction<UpdateArena>
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

		SpawnsetHistoryUtils.Save(SpawnsetEditType);
	}
}
