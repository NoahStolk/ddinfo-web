using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;

public sealed class HistoryWrapper : AbstractScrollViewer<HistoryWrapper, History>
{
	public HistoryWrapper(IBounds bounds)
		: base(bounds)
	{
		Rectangle historyMetric = Rectangle.At(0, 0, 240, 256);

		Content = new(historyMetric, this);
		Scrollbar = new(historyMetric with { X1 = historyMetric.X2, X2 = historyMetric.X2 + 16 }, SetScrollPercentage);

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
