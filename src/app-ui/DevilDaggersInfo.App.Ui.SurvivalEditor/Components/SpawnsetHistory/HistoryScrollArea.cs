using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;

public class HistoryScrollArea : ScrollArea
{
	private readonly List<AbstractComponent> _historyComponents = new();

	public HistoryScrollArea(IBounds bounds)
		: base(bounds, 96, 16, GlobalStyles.DefaultScrollAreaStyle)
	{
	}

	public void SetContent()
	{
		foreach (AbstractComponent component in _historyComponents)
			NestingContext.Remove(component);

		_historyComponents.Clear();

		for (int i = 0; i < SpawnsetHistoryManager.History.Count; i++)
		{
			States.SpawnsetHistoryState historyState = SpawnsetHistoryManager.History[i];
			bool isActive = i == SpawnsetHistoryManager.Index;
			Color colorBackground = historyState.EditType.GetColor();
			Color colorBackgroundActive = colorBackground.Intensify(32);
			Color hoverBackgroundColor = colorBackground.Intensify(64);
			ButtonStyle buttonStyle = new(isActive ? colorBackgroundActive : colorBackground, isActive ? Color.White : Color.Black, hoverBackgroundColor, 1);
			TextButtonStyle textButtonStyle = new(Color.White, TextAlign.Left, FontSize.H12);
			int index = i;
			const int historyEntryHeight = 16;
			TextButton button = new(Bounds.CreateNested(0, i * historyEntryHeight, Bounds.Size.X, historyEntryHeight), () => SpawnsetHistoryManager.Set(index), buttonStyle, textButtonStyle, historyState.EditType.GetChange())
			{
				Depth = Depth + 1,
			};
			_historyComponents.Add(button);
		}

		foreach (AbstractComponent component in _historyComponents)
			NestingContext.Add(component);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center, Depth, Color.Black);
	}
}
