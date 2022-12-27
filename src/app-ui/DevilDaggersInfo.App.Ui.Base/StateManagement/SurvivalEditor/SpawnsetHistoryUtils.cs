using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Core.Spawnset;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor;

public static class SpawnsetHistoryUtils
{
	private const int _maxHistoryEntries = 100;

	public static void Save(SpawnsetEditType spawnsetEditType)
	{
		// Clear any newer history.
		List<SpawnsetHistoryEntry> newHistory = StateManager.SpawnsetHistoryState.History.ToList();
		newHistory = newHistory.Take(StateManager.SpawnsetHistoryState.CurrentIndex + 1).ToList();

		SpawnsetBinary copy = StateManager.SpawnsetState.Spawnset.DeepCopy();
		byte[] originalHash = newHistory.Count == 0 ? Array.Empty<byte>() : newHistory[^1].Hash;
		byte[] hash = MD5.HashData(copy.ToBytes());

		if (ArrayUtils.AreEqual(originalHash, hash))
			return;

		newHistory.Add(new(copy, hash, spawnsetEditType));

		if (newHistory.Count > _maxHistoryEntries)
			newHistory.RemoveAt(0);

		StateManager.Dispatch(new SetHistory(newHistory));
		StateManager.Dispatch(new SetSpawnsetHistoryIndex(newHistory.Count - 1));
	}
}
