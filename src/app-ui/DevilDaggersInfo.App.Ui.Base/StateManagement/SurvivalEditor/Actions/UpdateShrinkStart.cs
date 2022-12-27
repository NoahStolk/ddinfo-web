using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateShrinkStart(float ShrinkStart) : IAction
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				ShrinkStart = ShrinkStart,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.ShrinkStart);
	}
}
