using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateSpawnsetSetting(SpawnsetBinary SpawnsetBinary) : IAction<UpdateSpawnsetSetting>
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = SpawnsetBinary,
		};
	}
}
