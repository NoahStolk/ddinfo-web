using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.Base.Rendering.Text;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using DevilDaggersInfo.Core.Wiki;
using Warp.NET.Extensions;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;

public class LeaderboardWrapper : AbstractComponent
{
	private static readonly Vector2 _iconSize = new(16);

	private readonly Label _spawnsetNameLabel;
	private readonly TextButton _playButton;
	private readonly LeaderboardScrollArea _leaderboardScrollArea;

	public LeaderboardWrapper(IBounds bounds)
		: base(bounds)
	{
		_spawnsetNameLabel = new(bounds.CreateNested(4, 4, 128, 16), string.Empty, LabelStyles.DefaultLeft);
		_leaderboardScrollArea = new(bounds.CreateNested(4, 48, 1016, 200));

		NestingContext.Add(_leaderboardScrollArea);

		_playButton = new(bounds.CreateNested(512, 4, 64, 24), DownloadAndInstallSpawnset, ButtonStyles.Default, new(Color.White, TextAlign.Middle, FontSize.H16), "Play")
		{
			IsDisabled = true,
		};
		NestingContext.Add(_playButton);

		StateManager.Subscribe<SetSelectedCustomLeaderboard>(SetSelectedCustomLeaderboard);

		void DownloadAndInstallSpawnset()
		{
			if (StateManager.LeaderboardListState.SelectedCustomLeaderboard == null)
				return;

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
				// TODO: Show error.
				_spawnsetNameLabel.Text = string.Empty;
				_leaderboardScrollArea.Clear();
			}
			else
			{
				_spawnsetNameLabel.Text = getCustomLeaderboard.SpawnsetName;
				_leaderboardScrollArea.SetContent(getCustomLeaderboard);
			}
		}
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth - 5, new(255, 127, 0, 255));
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), Bounds.Center + scrollOffset, Depth - 4, Color.Black);

		Root.Game.MonoSpaceFontRenderer24.Schedule(new(1), Bounds.TopLeft + new Vector2i<int>(4) + scrollOffset, Depth - 3, Color.White, StateManager.LeaderboardListState.SelectedCustomLeaderboard?.SpawnsetName ?? string.Empty, TextAlign.Left);

		Vector2 position = Bounds.TopLeft.ToVector2() + new Vector2(8, 36) + scrollOffset.ToVector2();
		const int offset = 8;
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[00] - offset, 0), Depth, WarpTextures.IconRank, Color.White);
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[01] + offset, 0), Depth, WarpTextures.IconEye, Color.White);
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[02] - offset, 0), Depth, WarpTextures.IconStopwatch, Color.White);
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[03] - offset, 0), Depth, WarpTextures.IconSkull, Color.Orange);
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[04] - offset, 0), Depth, WarpTextures.IconSkull, Color.Red);
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[05] - offset, 0), Depth, WarpTextures.IconGem, Color.Red);
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[06] - offset, 0), Depth, WarpTextures.IconGem, Color.Gray(0.5f));
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[07] - offset, 0), Depth, WarpTextures.IconGem, Color.Green);
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[08] - offset, 0), Depth, WarpTextures.IconCrosshair, Color.White);
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[09] + offset, 0), Depth, WarpTextures.IconSkull, Color.Gray(0.5f));
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[10] - offset, 0), Depth, WarpTextures.IconHoming, Color.White);
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[11] - offset, 0), Depth, WarpTextures.IconHomingMask, Color.Red);
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[12] - offset, 0), Depth, WarpTextures.IconDagger, UpgradesV3_2.Level2.Color.ToWarpColor());
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[13] - offset, 0), Depth, WarpTextures.IconDagger, UpgradesV3_2.Level3.Color.ToWarpColor());
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[14] - offset, 0), Depth, WarpTextures.IconDagger, UpgradesV3_2.Level4.Color.ToWarpColor());
		Root.Game.SpriteRenderer.Schedule(_iconSize, position + new Vector2(TableOffsets[15] - offset, 0), Depth, WarpTextures.IconCalendar, Color.White);
	}
}
