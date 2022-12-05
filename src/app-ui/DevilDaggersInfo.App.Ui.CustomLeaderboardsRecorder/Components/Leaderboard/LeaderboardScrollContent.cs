using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;

public sealed class LeaderboardScrollContent : ScrollContent<LeaderboardScrollContent, ScrollViewer<LeaderboardScrollContent>>, IScrollContent<LeaderboardScrollContent, ScrollViewer<LeaderboardScrollContent>>
{
	private readonly List<LeaderboardEntry> _leaderboardEntries = new();

	private LeaderboardScrollContent(IBounds bounds, ScrollViewer<LeaderboardScrollContent> parent)
		: base(bounds, parent)
	{
		Depth = 100;
	}

	public override int ContentHeightInPixels => _leaderboardEntries.Count * LeaderboardEntry.Height;

	public override void SetContent()
	{
		foreach (LeaderboardEntry leaderboardEntry in _leaderboardEntries)
			NestingContext.Remove(leaderboardEntry);

		_leaderboardEntries.Clear();

		if (StateManager.LeaderboardState.CustomLeaderboard == null)
			return;

		for (int i = 0; i < StateManager.LeaderboardState.CustomLeaderboard.SortedEntries.Count; i++)
		{
			GetCustomEntry customEntry = StateManager.LeaderboardState.CustomLeaderboard.SortedEntries[i];
			LeaderboardEntry leaderboardEntry = new(Bounds.CreateNested(4, 4 + i * LeaderboardEntry.Height, 256, LeaderboardEntry.Height), customEntry) { Depth = 101 };
			_leaderboardEntries.Add(leaderboardEntry);

			NestingContext.Add(leaderboardEntry);
		}
	}

	public static LeaderboardScrollContent Construct(IBounds bounds, ScrollViewer<LeaderboardScrollContent> parent)
	{
		return new(bounds, parent);
	}
}
