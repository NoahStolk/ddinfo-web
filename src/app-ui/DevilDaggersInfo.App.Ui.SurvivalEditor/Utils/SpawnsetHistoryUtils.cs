using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Core.Spawnset;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class SpawnsetHistoryUtils
{
	private const int _maxHistoryEntries = 100;

	public static void Save(SpawnsetEditType spawnsetEditType)
	{
		// Clear any newer history.
		List<SpawnsetHistoryEntry> newHistory = StateManager.SpawnsetHistoryState.History.ToList();
		newHistory = newHistory.Take(StateManager.SpawnsetHistoryState.CurrentIndex + 1).ToList();

		SpawnsetBinary copy = StateManager.SpawnsetState.Spawnset.DeepCopy();
		byte[] originalHash = StateManager.SpawnsetHistoryState.History.Count == 0 ? Array.Empty<byte>() : StateManager.SpawnsetHistoryState.History[^1].Hash;
		byte[] hash = MD5.HashData(copy.ToBytes());

		if (ArrayUtils.AreEqual(originalHash, hash))
			return;

		newHistory.Add(new(copy, hash, spawnsetEditType));

		if (newHistory.Count > _maxHistoryEntries)
			newHistory.RemoveAt(0);

		StateManager.Dispatch(new SetHistory(newHistory));
		StateManager.Dispatch(new SetSpawnsetHistoryIndex(StateManager.SpawnsetHistoryState.History.Count - 1));
	}
}
