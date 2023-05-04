using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.State;

public record SpawnsetHistoryEntry(SpawnsetBinary Spawnset, byte[] Hash, SpawnsetEditType EditType);
