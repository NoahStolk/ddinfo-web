namespace DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;

public record struct SoundSnapshot(int Tick, ReplaySound Sound, Vector3 Position);
