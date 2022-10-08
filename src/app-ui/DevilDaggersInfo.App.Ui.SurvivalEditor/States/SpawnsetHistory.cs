using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.States;

public record SpawnsetHistory(SpawnsetBinary Spawnset, byte[] Hash, SpawnsetEditType EditType);
