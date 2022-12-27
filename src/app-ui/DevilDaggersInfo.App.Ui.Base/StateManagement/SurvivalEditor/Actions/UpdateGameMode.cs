using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.Types.Core.Spawnsets;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateGameMode(GameMode GameMode) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				GameMode = GameMode,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.GameMode);
	}
}
