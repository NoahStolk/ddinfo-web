using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateShrinkEnd(float ShrinkEnd) : IAction
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				ShrinkEnd = ShrinkEnd,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.ShrinkEnd);
	}
}
