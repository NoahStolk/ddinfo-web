using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateFormat(int WorldVersion, int SpawnVersion) : IAction<UpdateFormat>
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				WorldVersion = WorldVersion,
				SpawnVersion = SpawnVersion,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.Format);
	}
}
