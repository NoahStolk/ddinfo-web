using DevilDaggersInfo.Core.Spawnset.View;
using DevilDaggersInfo.Types.Core.Spawnsets;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing;

// TODO: Mutable.
public readonly record struct EditableSpawn(EnemyType EnemyType, double Delay, double Seconds, int NoFarmGems, GemState GemState);
