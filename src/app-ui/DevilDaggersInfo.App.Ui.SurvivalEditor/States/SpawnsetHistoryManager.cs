using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Core.Spawnset;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.States;

public static class SpawnsetHistoryManager
{
	private const int _maxHistoryEntries = 100;
	private static readonly List<SpawnsetHistory> _history = new();

	private static bool _uiQueue;

	public static IReadOnlyList<SpawnsetHistory> History => _history;
	public static int Index { get; private set; }

	public static void Reset()
	{
		_history.Clear();
		Save(SpawnsetEditType.Reset);
	}

	public static void Save(SpawnsetEditType spawnsetEditType)
	{
		// Clear any newer history.
		for (int i = _history.Count - 1; i > Index; i--)
			_history.RemoveAt(i);

		SpawnsetBinary copy = StateManager.SpawnsetState.Spawnset.DeepCopy();
		byte[] originalHash = History.Count == 0 ? Array.Empty<byte>() : History[^1].Hash;
		byte[] hash = MD5.HashData(copy.ToBytes());

		if (ArrayUtils.AreEqual(originalHash, hash))
			return;

		_history.Add(new(copy, hash, spawnsetEditType));

		if (_history.Count > _maxHistoryEntries)
			_history.RemoveAt(0);

		Index = _history.Count - 1;

		_uiQueue = true;
	}

	public static void EmptyUiQueue()
	{
		if (!_uiQueue)
			return;

		Root.Game.SurvivalEditorMainLayout.SetHistory();
		_uiQueue = false;
	}

	public static void Undo()
	{
		if (Index <= 0)
			return;

		Index--;
		StateManager.SetSpawnset(_history[Index].Spawnset.DeepCopy());

		Root.Game.SurvivalEditorMainLayout.SetHistory();
	}

	public static void Redo()
	{
		if (Index >= _history.Count - 1)
			return;

		Index++;
		StateManager.SetSpawnset(_history[Index].Spawnset.DeepCopy());

		Root.Game.SurvivalEditorMainLayout.SetHistory();
	}
}
