using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateSpawnsetSetting(SpawnsetBinary SpawnsetBinary, SpawnsetEditType SpawnsetEditType) : IAction<UpdateSpawnsetSetting>
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = SpawnsetBinary,
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType);
	}
}
