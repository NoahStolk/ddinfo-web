using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Api.Main.Spawnsets;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using DevilDaggersInfo.Common;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class LeaderboardListEntry : AbstractComponent
{
	private readonly GetCustomLeaderboardForOverview _customLeaderboard;

	private bool _isHovering;

	public LeaderboardListEntry(IBounds bounds, GetCustomLeaderboardForOverview customLeaderboard)
		: base(bounds)
	{
		_customLeaderboard = customLeaderboard;

		int fullWidth = bounds.Size.X;
		int columnWidth = fullWidth / 6;

		const int gridIndex0 = 0;
		int gridIndex1 = columnWidth * 2;
		int gridIndex2 = columnWidth * 3;
		int gridIndex3 = columnWidth * 4;
		int gridIndex4 = columnWidth * 5;

		int labelDepth = Depth + 100;
		Label name = new(bounds.CreateNested(gridIndex0, 0, columnWidth * 2, bounds.Size.Y), customLeaderboard.SpawnsetName, GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };
		Label rank = new(bounds.CreateNested(gridIndex1, 0, columnWidth, bounds.Size.Y), $"{(customLeaderboard.SelectedPlayerStats?.Rank).ToString() ?? "-"} / {customLeaderboard.PlayerCount}", GlobalStyles.LabelDefaultRight) { Depth = labelDepth };
		Label score = new(bounds.CreateNested(gridIndex2, 0, columnWidth, bounds.Size.Y), customLeaderboard.SelectedPlayerStats?.Time.ToString(StringFormats.TimeFormat) ?? "-", GlobalStyles.LabelDefaultRight) { Depth = labelDepth };

		LabelStyle nextDaggerStyle = new(customLeaderboard.SelectedPlayerStats?.NextDagger?.Dagger.GetColor() ?? Color.White, TextAlign.Right, FontSize.H12);
		Label nextDagger = new(bounds.CreateNested(gridIndex3, 0, columnWidth, bounds.Size.Y), customLeaderboard.SelectedPlayerStats?.NextDagger?.Time.ToString(StringFormats.TimeFormat) ?? "-", nextDaggerStyle) { Depth = labelDepth };

		LabelStyle worldRecordStyle = new(customLeaderboard.WorldRecord?.Dagger?.GetColor() ?? Color.White, TextAlign.Right, FontSize.H12);
		Label worldRecord = new(bounds.CreateNested(gridIndex4, 0, columnWidth, bounds.Size.Y), customLeaderboard.WorldRecord?.Time.ToString(StringFormats.TimeFormat) ?? "-", worldRecordStyle) { Depth = labelDepth };

		NestingContext.Add(name);
		NestingContext.Add(rank);
		NestingContext.Add(score);
		NestingContext.Add(nextDagger);
		NestingContext.Add(worldRecord);
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		_isHovering = MouseUiContext.Contains(scrollOffset, Bounds);
		if (!_isHovering || !Input.IsButtonPressed(MouseButton.Left))
			return;

		StateManager.SetSelectedCustomLeaderboard(_customLeaderboard);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		if (_isHovering)
			Root.Game.RectangleRenderer.Schedule(Bounds.Size, scrollOffset + Bounds.Center, Depth - 1, GlobalColors.EntrySelect);
	}
}
