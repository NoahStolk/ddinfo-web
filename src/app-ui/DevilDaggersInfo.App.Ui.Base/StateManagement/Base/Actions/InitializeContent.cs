namespace DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;

/// <summary>
/// Fires when the installation has been verified and DD content has been found.
/// This action can only fire once.
/// </summary>
public record InitializeContent : IAction<InitializeContent>
{
	public void Reduce()
	{
	}
}
