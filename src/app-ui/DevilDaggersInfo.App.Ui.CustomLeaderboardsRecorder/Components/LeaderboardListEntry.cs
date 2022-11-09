using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Api.Main.Spawnsets;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.Common;
using Silk.NET.GLFW;
using Warp;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class LeaderboardListEntry : AbstractComponent
{
	private readonly GetCustomLeaderboardForOverview _customLeaderboard;

	private bool _isHovering;

	public LeaderboardListEntry(Rectangle metric, GetCustomLeaderboardForOverview customLeaderboard)
		: base(metric)
	{
		_customLeaderboard = customLeaderboard;

		int fullWidth = metric.Size.X;
		int columnWidth = fullWidth / 6;

		const int gridIndex0 = 0;
		int gridIndex1 = columnWidth * 2;
		int gridIndex2 = columnWidth * 3;
		int gridIndex3 = columnWidth * 4;
		int gridIndex4 = columnWidth * 5;

		Label name = new(Rectangle.At(gridIndex0, 0, columnWidth * 2, metric.Size.Y), Color.White, customLeaderboard.SpawnsetName, TextAlign.Left, FontSize.F8X8);
		Label rank = new(Rectangle.At(gridIndex1, 0, columnWidth, metric.Size.Y), Color.White, $"{(customLeaderboard.SelectedPlayerStats?.Rank).ToString() ?? "-"} / {customLeaderboard.PlayerCount}", TextAlign.Right, FontSize.F8X8);
		Label score = new(Rectangle.At(gridIndex2, 0, columnWidth, metric.Size.Y), Color.White, customLeaderboard.SelectedPlayerStats?.Time.ToString(StringFormats.TimeFormat) ?? "-", TextAlign.Right, FontSize.F8X8);
		Label nextDagger = new(Rectangle.At(gridIndex3, 0, columnWidth, metric.Size.Y), customLeaderboard.SelectedPlayerStats?.NextDagger?.Dagger.GetColor() ?? Color.White, customLeaderboard.SelectedPlayerStats?.NextDagger?.Time.ToString(StringFormats.TimeFormat) ?? "-", TextAlign.Right, FontSize.F8X8);
		Label worldRecord = new(Rectangle.At(gridIndex4, 0, columnWidth, metric.Size.Y), customLeaderboard.WorldRecord?.Dagger?.GetColor() ?? Color.White, customLeaderboard.WorldRecord?.Time.ToString(StringFormats.TimeFormat) ?? "-", TextAlign.Right, FontSize.F8X8);

		NestingContext.Add(name);
		NestingContext.Add(rank);
		NestingContext.Add(score);
		NestingContext.Add(nextDagger);
		NestingContext.Add(worldRecord);
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		_isHovering = MouseUiContext.Contains(parentPosition, Bounds);
		if (!_isHovering || !Input.IsButtonPressed(MouseButton.Left))
			return;

		AsyncHandler.Run(SetSpawnset, () => FetchSpawnset.HandleAsync(_customLeaderboard.SpawnsetId));

		void SetSpawnset(GetSpawnset? spawnset)
		{
			if (spawnset == null)
			{
				// TODO: Show error.
				return;
			}

			File.WriteAllBytes(Path.Combine(UserSettings.DevilDaggersInstallationDirectory, "mods", "survival"), spawnset.FileBytes);
		}
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		if (_isHovering)
			RenderBatchCollector.RenderRectangleTopLeft(Bounds.Size, parentPosition + Bounds.TopLeft, Depth - 1, new(127, 0, 0, 255));
	}
}
