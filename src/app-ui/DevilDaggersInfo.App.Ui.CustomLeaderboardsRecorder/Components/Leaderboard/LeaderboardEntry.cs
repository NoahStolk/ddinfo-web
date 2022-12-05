using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;

public class LeaderboardEntry : AbstractComponent
{
	public const int Height = 16;

	private readonly GetCustomEntry _getCustomEntry;

	public LeaderboardEntry(IBounds bounds, GetCustomEntry getCustomEntry)
		: base(bounds)
	{
		_getCustomEntry = getCustomEntry;
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), Bounds.TopLeft, Depth, Color.White, $"{_getCustomEntry.PlayerName} {_getCustomEntry.Time}", TextAlign.Left);
	}
}
