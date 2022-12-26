using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.States;

public record SpawnsetHistoryState(SpawnsetBinary Spawnset, byte[] Hash, SpawnsetEditType EditType);
