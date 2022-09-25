using DevilDaggersInfo.App.Ui.Base.Components;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;

public sealed class HistoryWrapper : AbstractScrollViewer<HistoryWrapper, History>
{
	public HistoryWrapper(Rectangle metric)
		: base(metric)
	{
		Rectangle historyMetric = Rectangle.At(0, 0, 480, 256);

		Content = new(historyMetric, this);
		Scrollbar = new(historyMetric with { X1 = historyMetric.X2, X2 = historyMetric.X2 + 16 }, ScrollbarOnChange);

		NestingContext.Add(Content);
		NestingContext.Add(Scrollbar);
	}

	protected override Scrollbar Scrollbar { get; }
	protected override History Content { get; }

	public override void InitializeContent()
	{
		Content.SetHistory();

		base.InitializeContent();
	}
}
