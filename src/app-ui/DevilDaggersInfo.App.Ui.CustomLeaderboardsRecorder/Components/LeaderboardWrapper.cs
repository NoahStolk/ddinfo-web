using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class LeaderboardWrapper : AbstractComponent
{
	public LeaderboardWrapper(IBounds bounds)
		: base(bounds)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.TopLeft + parentPosition, 0, new(255, 127, 0, 255));
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), Bounds.TopLeft + parentPosition + new Vector2i<int>(border), 1, Color.Black);
	}
}
