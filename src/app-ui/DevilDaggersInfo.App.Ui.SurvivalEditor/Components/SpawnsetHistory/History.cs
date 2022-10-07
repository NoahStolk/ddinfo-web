using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;

public class History : ScrollContent<History, HistoryWrapper>
{
	public const int HistoryEntryHeight = 16;

	private readonly List<AbstractComponent> _historyComponents = new();

	public History(Rectangle metric, HistoryWrapper historyWrapper)
		: base(metric, historyWrapper)
	{
	}

	public override int ContentHeightInPixels => _historyComponents.Count * HistoryEntryHeight;

	public void SetHistory()
	{
		foreach (AbstractComponent component in _historyComponents)
			NestingContext.Remove(component);

		_historyComponents.Clear();

		NestingContext.ScrollOffset = default;

		for (int i = 0; i < SpawnsetHistoryManager.History.Count; i++)
		{
			States.SpawnsetHistory history = SpawnsetHistoryManager.History[i];
			bool isActive = i == SpawnsetHistoryManager.Index;
			Color colorBackground = history.EditType.GetColor();
			Color colorBackgroundActive = colorBackground.Intensify(32);
			Color hoverBackgroundColor = colorBackground.Intensify(64);
			int index = i;
			Button button = new(Rectangle.At(0, i * HistoryEntryHeight, Metric.Size.X, HistoryEntryHeight), () => SpawnsetHistoryManager.Set(index), isActive ? colorBackgroundActive : colorBackground, isActive ? Color.White : Color.Black, hoverBackgroundColor, Color.White, history.EditType.GetChange(), TextAlign.Left, 2, FontSize.F8X8)
			{
				Depth = Depth + 1,
			};
			_historyComponents.Add(button);
		}

		foreach (AbstractComponent component in _historyComponents)
			NestingContext.Add(component);
	}
}
