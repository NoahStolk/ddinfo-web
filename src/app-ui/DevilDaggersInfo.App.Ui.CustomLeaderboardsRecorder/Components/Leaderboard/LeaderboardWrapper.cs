using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;

public class LeaderboardWrapper : AbstractComponent
{
	private readonly Label _label;
	private readonly TextButton _playButton;
	private readonly ScrollViewer<LeaderboardScrollContent> _leaderboardScrollViewer;

	public LeaderboardWrapper(IBounds bounds)
		: base(bounds)
	{
		_label = new(bounds.CreateNested(4, 4, 128, 16), string.Empty, LabelStyle.Default);
		_playButton = new(bounds.CreateNested(4, 32, 64, 24), DownloadSpawnset, GlobalStyles.DefaultButtonStyle, GlobalStyles.DefaultMiddle with { FontSize = FontSize.H16 }, "PLAY") { IsActive = false };
		_leaderboardScrollViewer = new(bounds.CreateNested(4, 64, 1016, 180), 16);

		NestingContext.Add(_playButton);
		NestingContext.Add(_leaderboardScrollViewer);
	}

	private static void DownloadSpawnset()
	{
		if (StateManager.LeaderboardListState.SelectedCustomLeaderboard == null)
		{
			// TODO: Log error.
			return;
		}

		AsyncHandler.Run(SetSpawnset, () => FetchSpawnsetById.HandleAsync(StateManager.LeaderboardListState.SelectedCustomLeaderboard.SpawnsetId));
		void SetSpawnset(GetSpawnset? spawnset)
		{
			if (spawnset == null)
			{
				// TODO: Show error.
				return;
			}

			File.WriteAllBytes(UserSettings.ModsSurvivalPath, spawnset.FileBytes);
		}
	}

	public void SetCustomLeaderboard()
	{
		_playButton.IsActive = StateManager.LeaderboardListState.SelectedCustomLeaderboard != null;
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
