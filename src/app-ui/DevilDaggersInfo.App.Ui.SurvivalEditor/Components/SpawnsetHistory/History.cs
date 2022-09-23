using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.OpenGL;
using Warp;
using Warp.Extensions;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;

public class History : ScrollContent
{
	private const int _scrollMultiplier = 64;
	public const int HistoryEntryHeight = 24;

	private readonly HistoryWrapper _historyWrapper;
	private readonly List<AbstractComponent> _historyComponents = new();

	public History(Rectangle metric, HistoryWrapper historyWrapper)
		: base(metric)
	{
		_historyWrapper = historyWrapper;
	}

	public override int ContentHeightInPixels => SpawnsetHistoryManager.History.Count * HistoryEntryHeight;

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		bool hoverWithoutBlock = Metric.Contains(MouseUiContext.MousePosition.RoundToVector2Int32() - parentPosition);
		if (!hoverWithoutBlock)
			return;

		int scroll = Input.GetScroll();
		if (scroll != 0)
			_historyWrapper.SetScroll(scroll * _scrollMultiplier);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		Gl.Enable(EnableCap.ScissorTest);
		SetScissor(parentPosition);

		Root.Game.UiRenderer.RenderTopLeft(Metric.Size, parentPosition + new Vector2i<int>(Metric.X1, Metric.Y1), Depth, Color.Black);

		base.Render(parentPosition);

		Gl.Disable(EnableCap.ScissorTest);
	}

	public override void RenderText(Vector2i<int> parentPosition)
	{
		Gl.Enable(EnableCap.ScissorTest);
		SetScissor(parentPosition);

		base.RenderText(parentPosition);

		Gl.Disable(EnableCap.ScissorTest);
	}

	private void SetScissor(Vector2i<int> parentPosition)
	{
		Vector2 viewportOffset = Root.Game.ViewportOffset;
		Gl.Scissor(Metric.X1 + (int)viewportOffset.X + parentPosition.X, Root.Game.InitialWindowHeight - (Metric.Size.Y + parentPosition.Y) + (int)viewportOffset.Y, (uint)Metric.Size.X, (uint)Metric.Size.Y);
	}

	public override void SetScrollOffset(Vector2i<int> scrollOffset)
	{
		NestingContext.ScrollOffset = Vector2i<int>.Clamp(scrollOffset, new(0, -_historyComponents.Count * HistoryEntryHeight + Metric.Size.Y), default);
	}

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
			Button button = new(Rectangle.At(0, i * HistoryEntryHeight, Metric.Size.X, HistoryEntryHeight), () => {}, isActive ? new(0, 127, 63, 255) : new(0, 63, 0, 255), Color.Black, isActive ? new(0, 191, 127, 255) : new(0, 127, 0, 255), Color.White, $"{Convert(history.Hash.Take(4))} {history.Change}", TextAlign.Left, 2, false);
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
