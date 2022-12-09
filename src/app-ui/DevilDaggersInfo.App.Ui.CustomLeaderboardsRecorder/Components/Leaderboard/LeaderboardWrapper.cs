using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using DevilDaggersInfo.Core.Wiki;
using Warp.NET.Extensions;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;

public class LeaderboardWrapper : AbstractComponent
{
	private readonly Label _label;
	private readonly ScrollViewer<LeaderboardScrollContent> _leaderboardScrollViewer;

	public LeaderboardWrapper(IBounds bounds)
		: base(bounds)
	{
		_label = new(bounds.CreateNested(4, 4, 128, 16), string.Empty, LabelStyle.Default);
		_leaderboardScrollViewer = new(bounds.CreateNested(4, 48, 1016, 200), 16);

		NestingContext.Add(_leaderboardScrollViewer);
	}


	public void SetCustomLeaderboard()
	{
		_label.Text = StateManager.LeaderboardListState.SelectedCustomLeaderboard?.SpawnsetName ?? string.Empty;

		_leaderboardScrollViewer.InitializeContent();
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth - 5, new(255, 127, 0, 255));
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), Bounds.Center + scrollOffset, Depth - 4, Color.Black);

		Root.Game.MonoSpaceFontRenderer24.Schedule(new(1), Bounds.TopLeft + new Vector2i<int>(4) + scrollOffset, Depth - 3, Color.White, StateManager.LeaderboardListState.SelectedCustomLeaderboard?.SpawnsetName ?? string.Empty, TextAlign.Left);
	}
}
