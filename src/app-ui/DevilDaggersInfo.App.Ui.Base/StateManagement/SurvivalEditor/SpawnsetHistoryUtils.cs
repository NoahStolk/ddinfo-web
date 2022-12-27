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
		if (spawnsetEditType == SpawnsetEditType.Reset)
		{
			SpawnsetBinary copy = StateManager.SpawnsetState.Spawnset.DeepCopy();
			byte[] hash = MD5.HashData(copy.ToBytes());
			StateManager.Dispatch(new SetHistory(new() { new(copy, hash, spawnsetEditType) }, 0));
		}
		else
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

			StateManager.Dispatch(new SetHistory(newHistory, newHistory.Count - 1));
		}
	}
}
