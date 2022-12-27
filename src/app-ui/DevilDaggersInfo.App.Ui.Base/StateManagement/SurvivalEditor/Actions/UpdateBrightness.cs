using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record UpdateBrightness(float Brightness) : IAction<UpdateBrightness>
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				Brightness = Brightness,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.Brightness);
	}
}
