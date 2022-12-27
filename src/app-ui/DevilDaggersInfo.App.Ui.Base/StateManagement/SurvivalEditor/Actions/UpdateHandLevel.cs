using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.Types.Core.Spawnsets;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateHandLevel(HandLevel HandLevel) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				HandLevel = HandLevel,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.HandLevel);
	}
}
