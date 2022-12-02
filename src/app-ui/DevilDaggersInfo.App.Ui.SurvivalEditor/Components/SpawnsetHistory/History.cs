using DevilDaggersInfo.App.Ui.SurvivalEditor.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;

public sealed class History : ScrollContent<History, ScrollViewer<History>>, IScrollContent<History, ScrollViewer<History>>
{
	private const int _historyEntryHeight = 16;

	private readonly List<AbstractComponent> _historyComponents = new();

	private History(IBounds bounds, ScrollViewer<History> historyWrapper)
		: base(bounds, historyWrapper)
	{
	}

	public override int ContentHeightInPixels => _historyComponents.Count * _historyEntryHeight;

	public override void SetContent()
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
			TextButtonStyle textButtonStyle = new(Color.White, TextAlign.Left, FontSize.H12);
			TextButton button = new(Bounds.CreateNested(0, i * _historyEntryHeight, Bounds.Size.X, _historyEntryHeight), () => SpawnsetHistoryManager.Set(index), buttonStyle, textButtonStyle, history.EditType.GetChange())
			{
				Depth = Depth + 1,
			};
			_historyComponents.Add(button);
		}

		foreach (AbstractComponent component in _historyComponents)
			NestingContext.Add(component);
	}

	public static History Construct(IBounds bounds, ScrollViewer<History> parent)
	{
		return new(bounds, parent);
	}
}
