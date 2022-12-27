using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateAdditionalGems(int AdditionalGems) : IAction<UpdateAdditionalGems>
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				AdditionalGems = AdditionalGems,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.AdditionalGems);
	}
}
