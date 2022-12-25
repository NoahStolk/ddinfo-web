using DevilDaggersInfo.App.Ui.Base.DependencyPattern;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record LoadLeaderboardList : IAction
{
	public void Reduce()
	{
		// TODO: The layout should subscribe to this action instead.
		Root.Game.CustomLeaderboardsRecorderMainLayout.RefreshLeaderboardList();
	}
}
