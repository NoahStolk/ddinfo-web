using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.States;

public record SpawnsetHistory(SpawnsetBinary Spawnset, byte[] Hash, string Change);
