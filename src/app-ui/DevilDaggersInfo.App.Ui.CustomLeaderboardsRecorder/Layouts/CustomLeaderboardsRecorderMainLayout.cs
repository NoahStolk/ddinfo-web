using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class CustomLeaderboardsRecorderMainLayout : Layout, IExtendedLayout
{
	public CustomLeaderboardsRecorderMainLayout()
		: base(Constants.Full)
	{
		Menu menu = new(new(0, 0, 1024, 16));
		NestingContext.Add(menu);
	}

	public void Update()
	{
	}

	public void Render3d()
	{
	}

	public void Render()
	{
	}
}
