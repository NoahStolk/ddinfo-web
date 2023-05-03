using DevilDaggersInfo.Core.Spawnset;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SpawnsetState
{
	public static SpawnsetBinary Spawnset { get; set; } = SpawnsetBinary.CreateDefault();

	public static string SpawnsetName { get; set; } = "(untitled)";

	// Note; the history should never be empty.
	public static IReadOnlyList<SpawnsetHistoryEntry> History { get; set; } = new List<SpawnsetHistoryEntry> { new(Spawnset.DeepCopy(), MD5.HashData(Spawnset.ToBytes()), SpawnsetEditType.Reset) };

	public static int CurrentHistoryIndex { get; set; }
}
