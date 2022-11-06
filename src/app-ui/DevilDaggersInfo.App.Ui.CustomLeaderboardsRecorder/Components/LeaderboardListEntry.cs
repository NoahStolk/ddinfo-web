using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class LeaderboardListEntry : AbstractComponent
{
	private readonly GetCustomLeaderboardForOverview _customLeaderboard;

	public LeaderboardListEntry(Rectangle metric, GetCustomLeaderboardForOverview customLeaderboard)
		: base(metric)
	{
		_customLeaderboard = customLeaderboard;

		TextButton nameButton = new(Rectangle.At(0, 0, metric.Size.X, metric.Size.Y), () => {}, GlobalStyles.DefaultButtonStyle, GlobalStyles.DefaultLeft, customLeaderboard.SpawnsetName);
		NestingContext.Add(nameButton);
	}
}
