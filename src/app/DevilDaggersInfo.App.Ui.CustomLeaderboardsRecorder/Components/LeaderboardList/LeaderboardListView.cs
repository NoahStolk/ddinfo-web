using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.LeaderboardList;

public class LeaderboardListView : AbstractComponent
{
	private const int _rowHeight = 16;
	private const int _borderSize = 1;

	private readonly List<LeaderboardListEntry> _leaderboardListEntries = new();

	public LeaderboardListView(IBounds bounds)
		: base(bounds)
	{
		LeaderboardListHeader header = new(bounds.CreateNested(_borderSize, 16, bounds.Size.X - _borderSize * 2, _rowHeight));
		NestingContext.Add(header);
	}

	public void Clear()
	{
		foreach (LeaderboardListEntry leaderboardListEntry in _leaderboardListEntries)
			NestingContext.Remove(leaderboardListEntry);

		_leaderboardListEntries.Clear();
	}

	public void Set()
	{
		int y = 32;
		foreach (GetCustomLeaderboardForOverview cl in StateManager.LeaderboardListState.PagedCustomLeaderboards)
		{
			_leaderboardListEntries.Add(new(Bounds.CreateNested(_borderSize, y, Bounds.Size.X - _borderSize * 2, _rowHeight), cl) { Depth = Depth + 3 });
			y += _rowHeight;
		}

		foreach (LeaderboardListEntry leaderboardListEntry in _leaderboardListEntries)
			NestingContext.Add(leaderboardListEntry);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		string text;
		Color color;
		if (StateManager.LeaderboardListState.IsLoading)
		{
			text = "Loading...";
			color = Color.Red;
		}
		else
		{
			int total = StateManager.LeaderboardListState.GetTotal();
			int totalPages = StateManager.LeaderboardListState.GetTotalPages();

			int page = StateManager.LeaderboardListState.PageIndex + 1;
			int start = StateManager.LeaderboardListState.PageIndex * Constants.CustomLeaderboardsPageSize + 1;
			int end = Math.Min(total, (StateManager.LeaderboardListState.PageIndex + 1) * Constants.CustomLeaderboardsPageSize);
			text = $"Page {page} of {totalPages} ({start} - {end} of {total})";
			color = Color.Yellow;
		}

		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), scrollOffset + Bounds.TopLeft + new Vector2i<int>(4, 0), Depth + 2, color, text, TextAlign.Left);
	}
}
