using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;

public class LeaderboardScrollArea : ScrollArea
{
	private readonly List<LeaderboardEntry> _leaderboardEntries = new();

	public LeaderboardScrollArea(IBounds bounds)
		: base(bounds, 96, 16, GlobalStyles.DefaultScrollAreaStyle)
	{
		Depth = 100;
	}

	public void SetContent()
	{
		foreach (LeaderboardEntry leaderboardEntry in _leaderboardEntries)
			NestingContext.Remove(leaderboardEntry);

		_leaderboardEntries.Clear();

		if (StateManager.LeaderboardState.CustomLeaderboard == null)
			return;

		for (int i = 0; i < StateManager.LeaderboardState.CustomLeaderboard.SortedEntries.Count; i++)
		{
			GetCustomEntry customEntry = StateManager.LeaderboardState.CustomLeaderboard.SortedEntries[i];
			LeaderboardEntry leaderboardEntry = new(Bounds.CreateNested(4, i * LeaderboardEntry.Height, 996, LeaderboardEntry.Height), customEntry) { Depth = 101 };
			_leaderboardEntries.Add(leaderboardEntry);

			NestingContext.Add(leaderboardEntry);
		}
	}
}
