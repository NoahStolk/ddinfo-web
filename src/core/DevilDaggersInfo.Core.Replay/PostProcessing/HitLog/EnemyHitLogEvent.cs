using DevilDaggersInfo.Types.Core.Replays;

namespace DevilDaggersInfo.Core.Replay.PostProcessing.HitLog;

public readonly record struct EnemyHitLogEvent(int Tick, int Hp, int Damage, DaggerType DaggerType, int UserData);
