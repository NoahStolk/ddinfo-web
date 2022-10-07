using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;

public sealed class HistoryWrapper : AbstractScrollViewer<HistoryWrapper, History>
{
	public HistoryWrapper(Rectangle metric)
		: base(metric)
	{
		Rectangle historyMetric = Rectangle.At(0, 0, 240, 256);

		Content = new(historyMetric, this);
		Scrollbar = new(historyMetric with { X1 = historyMetric.X2, X2 = historyMetric.X2 + 16 }, ScrollbarOnChange);

		NestingContext.Add(Content);
		NestingContext.Add(Scrollbar);
	}

	public override Scrollbar Scrollbar { get; }
	public override History Content { get; }

	public override void InitializeContent()
	{
		Content.SetHistory();

		SetThumbPercentageSize();
		SetScrollPercentage(SpawnsetHistoryManager.Index / (float)SpawnsetHistoryManager.History.Count);
	}
}
