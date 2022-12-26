namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

/// <summary>
/// Fires when the current spawnset has to be written to the mods directory.
/// </summary>
public record ReplaceCurrentlyActiveSpawnset : IAction<ReplaceCurrentlyActiveSpawnset>
{
	public void Reduce()
	{
	}
}
