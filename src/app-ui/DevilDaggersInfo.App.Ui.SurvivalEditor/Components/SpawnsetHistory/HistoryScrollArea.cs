using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Extensions;
using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Core.Spawnset;
using System.Security.Cryptography;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;

public class HistoryScrollArea : ScrollArea
{
	private const int _maxHistoryEntries = 100;

	private readonly List<SpawnsetHistoryStateEntry> _history = new();

	private readonly List<AbstractComponent> _historyComponents = new();

	public HistoryScrollArea(IBounds bounds)
		: base(bounds, 96, 16, GlobalStyles.DefaultScrollAreaStyle)
	{
		StateManager.Subscribe<LoadSpawnset>(_ => Reset());
		StateManager.Subscribe<SaveHistory>(Save);
		StateManager.Subscribe<SetSpawnsetHistoryIndex>(SetSpawnsetHistoryIndex); // TODO: Subscribe to more actions.
	}

	private void Reset()
	{
		_history.Clear();
		StateManager.Dispatch(new SaveHistory(SpawnsetEditType.Reset));
	}

	private void Save(SaveHistory saveHistory)
	{
		// Clear any newer history.
		for (int i = _history.Count - 1; i > StateManager.SpawnsetHistoryState.CurrentIndex; i--)
			_history.RemoveAt(i);

		SpawnsetBinary copy = StateManager.SpawnsetState.Spawnset.DeepCopy();
		byte[] originalHash = _history.Count == 0 ? Array.Empty<byte>() : _history[^1].Hash;
		byte[] hash = MD5.HashData(copy.ToBytes());

		if (ArrayUtils.AreEqual(originalHash, hash))
			return;

		_history.Add(new(copy, hash, saveHistory.SpawnsetEditType));

		if (_history.Count > _maxHistoryEntries)
			_history.RemoveAt(0);

		StateManager.Dispatch(new SetSpawnsetHistoryIndex(_history.Count - 1, _history.Count));
	}

	private void SetSpawnsetHistoryIndex(SetSpawnsetHistoryIndex setSpawnsetHistoryIndex)
	{
		StateManager.Dispatch(new LoadSpawnsetFromHistory(_history[setSpawnsetHistoryIndex.Index].Spawnset.DeepCopy()));

		SetContent();
	}

	private void SetContent()
	{
		foreach (AbstractComponent component in _historyComponents)
			NestingContext.Remove(component);

		_historyComponents.Clear();

		for (int i = 0; i < _history.Count; i++)
		{
			SpawnsetHistoryStateEntry historyStateEntry = _history[i];
			bool isActive = i == StateManager.SpawnsetHistoryState.CurrentIndex;
			Color colorBackground = historyStateEntry.EditType.GetColor();
			Color colorBackgroundActive = colorBackground.Intensify(32);
			Color hoverBackgroundColor = colorBackground.Intensify(64);
			ButtonStyle buttonStyle = new(isActive ? colorBackgroundActive : colorBackground, isActive ? Color.White : Color.Black, hoverBackgroundColor, 1);
			TextButtonStyle textButtonStyle = new(Color.White, TextAlign.Left, FontSize.H12);
			int index = i;
			const int historyEntryHeight = 16;
			TextButton button = new(Bounds.CreateNested(0, i * historyEntryHeight, Bounds.Size.X, historyEntryHeight), () => StateManager.Dispatch(new SetSpawnsetHistoryIndex(index, _history.Count)), buttonStyle, textButtonStyle, historyStateEntry.EditType.GetChange())
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

	private sealed record SpawnsetHistoryStateEntry(SpawnsetBinary Spawnset, byte[] Hash, SpawnsetEditType EditType);
}
