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
		Label processLabel = new(Rectangle.At(0, 0, 64, 16), "Process", GlobalStyles.LabelDefaultLeft);
		Label memoryLabel = new(Rectangle.At(0, 16, 64, 16), "Memory", GlobalStyles.LabelDefaultLeft);
		Label stateLabel = new(Rectangle.At(0, 32, 64, 16), "State", GlobalStyles.LabelDefaultLeft);
		Label submissionLabel = new(Rectangle.At(0, 48, 64, 16), "Submission", GlobalStyles.LabelDefaultLeft);

		NestingContext.Add(processLabel);
		NestingContext.Add(memoryLabel);
		NestingContext.Add(stateLabel);
		NestingContext.Add(submissionLabel);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 1;
		Vector2i<int> center = Bounds.TopLeft + Bounds.Size / 2;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, center + parentPosition, 0, Color.Red);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), center + parentPosition, 1, Color.Black);
	}
}
