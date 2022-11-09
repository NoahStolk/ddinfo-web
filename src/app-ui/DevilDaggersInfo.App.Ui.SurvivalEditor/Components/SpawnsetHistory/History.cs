using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;

public class History : ScrollContent<History, HistoryWrapper>
{
	private const int _historyEntryHeight = 16;

	private readonly List<AbstractComponent> _historyComponents = new();

	public History(Rectangle metric, HistoryWrapper historyWrapper)
		: base(metric, historyWrapper)
	{
	}

	public override int ContentHeightInPixels => _historyComponents.Count * _historyEntryHeight;

	public void SetHistory()
	{
		foreach (AbstractComponent component in _historyComponents)
			NestingContext.Remove(component);

		_historyComponents.Clear();

		for (int i = 0; i < SpawnsetHistoryManager.History.Count; i++)
		{
			States.SpawnsetHistory history = SpawnsetHistoryManager.History[i];
			bool isActive = i == SpawnsetHistoryManager.Index;
			Color colorBackground = history.EditType.GetColor();
			Color colorBackgroundActive = colorBackground.Intensify(32);
			Color hoverBackgroundColor = colorBackground.Intensify(64);
			int index = i;
			ButtonStyle buttonStyle = new(isActive ? colorBackgroundActive : colorBackground, isActive ? Color.White : Color.Black, hoverBackgroundColor, 1);
			TextButtonStyle textButtonStyle = new(Color.White, TextAlign.Left, FontSize.F8X8);
			TextButton button = new(Rectangle.At(0, i * _historyEntryHeight, Bounds.Size.X, _historyEntryHeight), () => SpawnsetHistoryManager.Set(index), buttonStyle, textButtonStyle, history.EditType.GetChange())
			{
				Depth = Depth + 1,
			};
			_historyComponents.Add(button);
		}

		foreach (AbstractComponent component in _historyComponents)
			NestingContext.Add(component);
	}
}
