using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Core.Spawnset;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SpawnsetHistoryUtils
{
	private const int _maxHistoryEntries = 100;

	public static void Save(SpawnsetEditType spawnsetEditType)
	{
		if (spawnsetEditType == SpawnsetEditType.Reset)
		{
			SpawnsetBinary copy = SpawnsetState.Spawnset.DeepCopy();
			byte[] hash = MD5.HashData(copy.ToBytes());
			SpawnsetState.History = new List<SpawnsetHistoryEntry> { new(copy, hash, spawnsetEditType) };
		}
		else
		{
			SpawnsetBinary copy = SpawnsetState.Spawnset.DeepCopy();
			byte[] originalHash = SpawnsetState.History[SpawnsetState.CurrentHistoryIndex].Hash;
			byte[] hash = MD5.HashData(copy.ToBytes());

			if (ArrayUtils.AreEqual(originalHash, hash))
				return;

			SpawnsetHistoryEntry historyEntry = new(copy, hash, spawnsetEditType);

			// Clear any newer history.
			List<SpawnsetHistoryEntry> newHistory = SpawnsetState.History.ToList();
			newHistory = newHistory.Take(SpawnsetState.CurrentHistoryIndex + 1).Append(historyEntry).ToList();

			// Remove history if there are too many entries.
			int newCurrentIndex = SpawnsetState.CurrentHistoryIndex + 1;
			if (newHistory.Count > _maxHistoryEntries)
			{
				newHistory.RemoveAt(0);
				newCurrentIndex--;
			}

			SpawnsetState.History = newHistory;
			SpawnsetState.CurrentHistoryIndex = newCurrentIndex;
		}
	}
}
