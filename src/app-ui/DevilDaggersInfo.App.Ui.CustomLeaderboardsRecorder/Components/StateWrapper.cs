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
		int labelDepth = Depth + 2;
		Label processLabel = new(bounds.CreateNested(0, 0, 64, 16), "Process", GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };
		Label memoryLabel = new(bounds.CreateNested(0, 16, 64, 16), "Memory", GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };
		Label stateLabel = new(bounds.CreateNested(0, 32, 64, 16), "State", GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };
		Label submissionLabel = new(bounds.CreateNested(0, 48, 64, 16), "Submission", GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };

		NestingContext.Add(processLabel);
		NestingContext.Add(memoryLabel);
		NestingContext.Add(stateLabel);
		NestingContext.Add(submissionLabel);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth, Color.Red);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), Bounds.Center + scrollOffset, Depth + 1, Color.Black);
	}
}
