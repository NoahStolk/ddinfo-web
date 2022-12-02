using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class StateWrapper : AbstractComponent
{
	public StateWrapper(IBounds bounds)
		: base(bounds)
	{
		Label processLabel = new(new PixelBounds(0, 0, 64, 16), "Process", GlobalStyles.LabelDefaultLeft);
		Label memoryLabel = new(new PixelBounds(0, 16, 64, 16), "Memory", GlobalStyles.LabelDefaultLeft);
		Label stateLabel = new(new PixelBounds(0, 32, 64, 16), "State", GlobalStyles.LabelDefaultLeft);
		Label submissionLabel = new(new PixelBounds(0, 48, 64, 16), "Submission", GlobalStyles.LabelDefaultLeft);

		NestingContext.Add(processLabel);
		NestingContext.Add(memoryLabel);
		NestingContext.Add(stateLabel);
		NestingContext.Add(submissionLabel);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + parentPosition, 0, Color.Red);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), Bounds.Center + parentPosition, 1, Color.Black);
	}
}
