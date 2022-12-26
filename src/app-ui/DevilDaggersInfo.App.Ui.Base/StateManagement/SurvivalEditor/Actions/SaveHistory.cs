using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SaveHistory(SpawnsetEditType SpawnsetEditType) : IAction<SaveHistory>
{
	public void Reduce()
	{
	}
}
