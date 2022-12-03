using DevilDaggersInfo.Api.Main.Spawnsets;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class LeaderboardWrapper : AbstractComponent
{
	private readonly TextButton _installButton;

	public LeaderboardWrapper(IBounds bounds)
		: base(bounds)
	{
		_installButton = new(bounds.CreateNested(4, 4, 128, 16), DownloadSpawnset, GlobalStyles.DefaultButtonStyle, GlobalStyles.DefaultMiddle, "Install");
		NestingContext.Add(_installButton);
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

	// TODO: Call this when selecting a CL.
	public void SetCustomLeaderboard()
	{
		_installButton.IsActive = StateManager.LeaderboardListState.SelectedCustomLeaderboard == null;

		// TODO: Show leaderboard and maybe arena.
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth, new(255, 127, 0, 255));
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), Bounds.Center + scrollOffset, Depth + 1, Color.Black);

		Root.Game.MonoSpaceFontRenderer24.Schedule(new(1), Bounds.Center + scrollOffset, Depth, Color.White, StateManager.LeaderboardListState.SelectedCustomLeaderboard?.SpawnsetName ?? string.Empty, TextAlign.Middle);
	}
}
