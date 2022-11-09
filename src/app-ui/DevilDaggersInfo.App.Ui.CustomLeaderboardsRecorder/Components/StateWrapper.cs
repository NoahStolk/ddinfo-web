using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class StateWrapper : AbstractComponent
{
	public StateWrapper(Rectangle metric)
		: base(metric)
	{
		Label processLabel = new(Rectangle.At(0, 0, 64, 16), Color.White, "Process", TextAlign.Left, FontSize.F8X8);
		Label memoryLabel = new(Rectangle.At(0, 16, 64, 16), Color.White, "Memory", TextAlign.Left, FontSize.F8X8);
		Label stateLabel = new(Rectangle.At(0, 32, 64, 16), Color.White, "State", TextAlign.Left, FontSize.F8X8);
		Label submissionLabel = new(Rectangle.At(0, 48, 64, 16), Color.White, "Submission", TextAlign.Left, FontSize.F8X8);

		NestingContext.Add(processLabel);
		NestingContext.Add(memoryLabel);
		NestingContext.Add(stateLabel);
		NestingContext.Add(submissionLabel);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 1;
		RenderBatchCollector.RenderRectangleTopLeft(Bounds.Size, Bounds.TopLeft + parentPosition, 0, Color.Red);
		RenderBatchCollector.RenderRectangleTopLeft(Bounds.Size - new Vector2i<int>(border * 2), Bounds.TopLeft + parentPosition + new Vector2i<int>(border), 1, Color.Black);
	}
}
