using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
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
			Button button = new(Rectangle.At(0, i * HistoryEntryHeight, Metric.Size.X, HistoryEntryHeight), () => {}, isActive ? new(0, 127, 63, 255) : new(0, 63, 0, 255), Color.Black, isActive ? new(0, 191, 127, 255) : new(0, 127, 0, 255), Color.White, $"{Convert(history.Hash.Take(4))} {history.Change}", TextAlign.Left, 2, FontSize.F8X8);
			_historyComponents.Add(button);
		}

		foreach (AbstractComponent component in _historyComponents)
			NestingContext.Add(component);

		string Convert(IEnumerable<byte> a)
		{
			return string.Join("", a.Select(b => $"{b:X2}"));
		}
	}
}
