using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Api.App.Spawnsets;
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

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.LeaderboardList;

public class LeaderboardListEntry : AbstractComponent
{
	private readonly GetCustomLeaderboardForOverview _customLeaderboard;

	private bool _isHovering;

	public LeaderboardListEntry(IBounds bounds, GetCustomLeaderboardForOverview customLeaderboard)
		: base(bounds)
	{
		_customLeaderboard = customLeaderboard;

		int fullWidth = bounds.Size.X;
		int columnWidth = fullWidth / 10;

		const int gridIndexName = 0;
		int gridIndexRank = columnWidth * 5;
		int gridIndexScore = columnWidth * 6;
		int gridIndexNextDagger = columnWidth * 7;
		int gridIndexPlayers = columnWidth * 8;
		int gridIndexWorldRecord = columnWidth * 9;

		int iconOffset = 0;
		foreach (GetCustomLeaderboardCriteria criteria in customLeaderboard.Criteria)
		{
			CriteriaIcon icon = new(bounds.CreateNested(columnWidth * 4 + iconOffset, 0, 16, 16), criteria) { Depth = Depth + 102 };
			NestingContext.Add(icon);

			iconOffset += 16;
		}

		LabelStyle scoreStyle = new(customLeaderboard.SelectedPlayerStats?.Dagger?.GetColor() ?? Color.White, TextAlign.Right, FontSize.H12);
		LabelStyle nextDaggerStyle = new(customLeaderboard.SelectedPlayerStats?.NextDagger?.Dagger.GetColor() ?? Color.White, TextAlign.Right, FontSize.H12);
		LabelStyle worldRecordStyle = new(customLeaderboard.WorldRecord?.Dagger?.GetColor() ?? Color.White, TextAlign.Right, FontSize.H12);

		int labelDepth = Depth + 100;
		Label name = new(bounds.CreateNested(gridIndexName, 0, columnWidth * 2, bounds.Size.Y), customLeaderboard.SpawnsetName, GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };
		Label rank = new(bounds.CreateNested(gridIndexRank, 0, columnWidth, bounds.Size.Y), customLeaderboard.SelectedPlayerStats == null ? "-" : $"{customLeaderboard.SelectedPlayerStats.Rank,2} / {customLeaderboard.PlayerCount,2}", GlobalStyles.LabelDefaultRight) { Depth = labelDepth };
		Label score = new(bounds.CreateNested(gridIndexScore, 0, columnWidth, bounds.Size.Y), customLeaderboard.SelectedPlayerStats?.Time.ToString(StringFormats.TimeFormat) ?? "-", scoreStyle) { Depth = labelDepth };
		Label nextDagger = new(bounds.CreateNested(gridIndexNextDagger, 0, columnWidth, bounds.Size.Y), customLeaderboard.SelectedPlayerStats?.NextDagger?.Time.ToString(StringFormats.TimeFormat) ?? "-", nextDaggerStyle) { Depth = labelDepth };
		Label players = new(bounds.CreateNested(gridIndexPlayers, 0, columnWidth, bounds.Size.Y), customLeaderboard.PlayerCount.ToString(), GlobalStyles.LabelDefaultRight) { Depth = labelDepth };
		Label worldRecord = new(bounds.CreateNested(gridIndexWorldRecord, 0, columnWidth, bounds.Size.Y), customLeaderboard.WorldRecord?.Time.ToString(StringFormats.TimeFormat) ?? "-", worldRecordStyle) { Depth = labelDepth };

		NestingContext.Add(name);
		NestingContext.Add(rank);
		NestingContext.Add(players);
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
		DownloadAndInstallSpawnset();
	}

	private static void DownloadAndInstallSpawnset()
	{
		if (StateManager.LeaderboardListState.SelectedCustomLeaderboard == null)
		{
			// TODO: Log error.
			return;
		}

		AsyncHandler.Run(InstallSpawnset, () => FetchSpawnsetById.HandleAsync(StateManager.LeaderboardListState.SelectedCustomLeaderboard.SpawnsetId));
		void InstallSpawnset(GetSpawnset? spawnset)
		{
			if (spawnset == null)
			{
				// TODO: Show error.
				return;
			}

			File.WriteAllBytes(UserSettings.ModsSurvivalPath, spawnset.FileBytes);
		}
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		if (_isHovering)
			Root.Game.RectangleRenderer.Schedule(Bounds.Size, scrollOffset + Bounds.Center, Depth - 1, GlobalColors.EntrySelect);
	}
}
