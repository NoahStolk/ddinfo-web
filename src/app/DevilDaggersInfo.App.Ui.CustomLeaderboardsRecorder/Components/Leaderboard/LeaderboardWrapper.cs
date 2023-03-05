using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.Base.Networking;
using DevilDaggersInfo.App.Ui.Base.Networking.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base.Rendering.Text;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using DevilDaggersInfo.App.Ui.Base.User.Settings;
using DevilDaggersInfo.Core.Wiki;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;

public class LeaderboardWrapper : AbstractComponent
{
	private readonly TextButton _playButton;
	private readonly LeaderboardScrollArea _leaderboardScrollArea;

	public LeaderboardWrapper(IBounds bounds)
		: base(bounds)
	{
		_playButton = new(bounds.CreateNested(4, 36, 64, 24), DownloadAndInstallSpawnset, ButtonStyles.Default, new(Color.White, TextAlign.Middle, FontSize.H16), "Play") { IsDisabled = true };
		_leaderboardScrollArea = new(bounds.CreateNested(4, 84, 1016, bounds.Size.Y - 88));

		NestingContext.Add(_playButton);
		NestingContext.Add(_leaderboardScrollArea);

		const int iconSize = 16;
		const int offset = 8;
		TooltipSprite rankTooltip = new(Bounds.CreateNested(TableOffsets[00] - offset, 64, iconSize, iconSize), WarpTextures.IconRank, Color.White, "Rank");
		TooltipSprite nameTooltip = new(Bounds.CreateNested(TableOffsets[01] + offset, 64, iconSize, iconSize), WarpTextures.IconEye, Color.White, "Name");
		TooltipSprite scoreTooltip = new(Bounds.CreateNested(TableOffsets[02] - offset, 64, iconSize, iconSize), WarpTextures.IconStopwatch, Color.White, "Score");
		TooltipSprite enemiesAliveTooltip = new(Bounds.CreateNested(TableOffsets[03] - offset, 64, iconSize, iconSize), WarpTextures.IconSkull, Color.Orange, "Enemies alive");
		TooltipSprite enemiesKilledTooltip = new(Bounds.CreateNested(TableOffsets[04] - offset, 64, iconSize, iconSize), WarpTextures.IconSkull, Color.Red, "Enemies killed");
		TooltipSprite gemsCollectedTooltip = new(Bounds.CreateNested(TableOffsets[05] - offset, 64, iconSize, iconSize), WarpTextures.IconGem, Color.Red, "Gems collected");
		TooltipSprite gemsDespawnedTooltip = new(Bounds.CreateNested(TableOffsets[06] - offset, 64, iconSize, iconSize), WarpTextures.IconGem, Color.Gray(0.5f), "Gems despawned");
		TooltipSprite gemsEatenTooltip = new(Bounds.CreateNested(TableOffsets[07] - offset, 64, iconSize, iconSize), WarpTextures.IconGem, Color.Green, "Gems eaten");
		TooltipSprite accuracyTooltip = new(Bounds.CreateNested(TableOffsets[08] - offset, 64, iconSize, iconSize), WarpTextures.IconCrosshair, Color.White, "Accuracy");
		TooltipSprite deathTooltip = new(Bounds.CreateNested(TableOffsets[09] + offset, 64, iconSize, iconSize), WarpTextures.IconSkull, Color.Gray(0.5f), "Death type");
		TooltipSprite homingStoredTooltip = new(Bounds.CreateNested(TableOffsets[10] - offset, 64, iconSize, iconSize), WarpTextures.IconHoming, Color.White, "Homing stored");
		TooltipSprite homingEatenTooltip = new(Bounds.CreateNested(TableOffsets[11] - offset, 64, iconSize, iconSize), WarpTextures.IconHomingMask, Color.Red, "Homing eaten");
		TooltipSprite level2Tooltip = new(Bounds.CreateNested(TableOffsets[12] - offset, 64, iconSize, iconSize), WarpTextures.IconDagger, UpgradesV3_2.Level2.Color.ToWarpColor(), "Level 2");
		TooltipSprite level3Tooltip = new(Bounds.CreateNested(TableOffsets[13] - offset, 64, iconSize, iconSize), WarpTextures.IconDagger, UpgradesV3_2.Level3.Color.ToWarpColor(), "Level 3");
		TooltipSprite level4Tooltip = new(Bounds.CreateNested(TableOffsets[14] - offset, 64, iconSize, iconSize), WarpTextures.IconDagger, UpgradesV3_2.Level4.Color.ToWarpColor(), "Level 4");
		TooltipSprite submitDateTooltip = new(Bounds.CreateNested(TableOffsets[15] - offset, 64, iconSize, iconSize), WarpTextures.IconCalendar, Color.White, "Submit date");

		NestingContext.Add(rankTooltip);
		NestingContext.Add(nameTooltip);
		NestingContext.Add(scoreTooltip);
		NestingContext.Add(enemiesAliveTooltip);
		NestingContext.Add(enemiesKilledTooltip);
		NestingContext.Add(gemsCollectedTooltip);
		NestingContext.Add(gemsDespawnedTooltip);
		NestingContext.Add(gemsEatenTooltip);
		NestingContext.Add(accuracyTooltip);
		NestingContext.Add(deathTooltip);
		NestingContext.Add(homingStoredTooltip);
		NestingContext.Add(homingEatenTooltip);
		NestingContext.Add(level2Tooltip);
		NestingContext.Add(level3Tooltip);
		NestingContext.Add(level4Tooltip);
		NestingContext.Add(submitDateTooltip);

		StateManager.Subscribe<SetSelectedCustomLeaderboard>(SetSelectedCustomLeaderboard);
		StateManager.Subscribe<SetSuccessfulUpload>(SetCustomLeaderboardFromUploadResponse);

		void DownloadAndInstallSpawnset()
		{
			if (StateManager.LeaderboardListState.SelectedCustomLeaderboard == null)
				return;

			AsyncHandler.Run(InstallSpawnset, () => FetchSpawnsetById.HandleAsync(StateManager.LeaderboardListState.SelectedCustomLeaderboard.SpawnsetId));
			void InstallSpawnset(GetSpawnset? spawnset)
			{
				if (spawnset == null)
				{
					Root.Dependencies.NativeDialogService.ReportError("Could not fetch spawnset.");
					return;
				}

				File.WriteAllBytes(UserSettings.ModsSurvivalPath, spawnset.FileBytes);
			}
		}
	}

	public static IReadOnlyList<int> TableOffsets { get; } = new List<int> { 16, 24, 260, 308, 352, 400, 448, 496, 552, 560, 664, 720, 776, 832, 888, 992 };

	private void SetSelectedCustomLeaderboard()
	{
		if (StateManager.LeaderboardListState.SelectedCustomLeaderboard == null)
			return;

		AsyncHandler.Run(UpdateDisplayedCustomLeaderboard, () => FetchCustomLeaderboardById.HandleAsync(StateManager.LeaderboardListState.SelectedCustomLeaderboard.Id));

		void UpdateDisplayedCustomLeaderboard(GetCustomLeaderboard? getCustomLeaderboard)
		{
			_playButton.IsDisabled = getCustomLeaderboard == null;

			if (getCustomLeaderboard == null)
			{
				Root.Dependencies.NativeDialogService.ReportError("Could not fetch custom leaderboard.");
				_leaderboardScrollArea.Clear();
			}
			else
			{
				_leaderboardScrollArea.SetContent(getCustomLeaderboard.SortedEntries);
			}
		}
	}

	private void SetCustomLeaderboardFromUploadResponse()
	{
		if (StateManager.UploadResponseState.UploadResponse?.NewSortedEntries == null)
			return;

		_playButton.IsDisabled = false;
		_leaderboardScrollArea.SetContent(StateManager.UploadResponseState.UploadResponse.NewSortedEntries);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		Root.Game.RectangleRenderer.Schedule(new(Bounds.Size.X, 2), new Vector2i<int>(Bounds.Center.X, Bounds.Y1) + scrollOffset, Depth - 5, Color.Gray(0.4f));

		if (StateManager.LeaderboardListState.SelectedCustomLeaderboard == null)
			return;

		base.Render(scrollOffset);

		Root.Game.MonoSpaceFontRenderer24.Schedule(new(1), Bounds.TopLeft + new Vector2i<int>(4) + scrollOffset, Depth - 3, Color.White, StateManager.LeaderboardListState.SelectedCustomLeaderboard.SpawnsetName, TextAlign.Left);
	}
}
