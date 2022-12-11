namespace DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;

public record ReplaySimulation
{
	public ReplaySimulation(IReadOnlyList<PlayerMovementSnapshot> movementSnapshots, IReadOnlyList<SoundSnapshot> soundSnapshots)
	{
		if (movementSnapshots.Count == 0)
			throw new InvalidOperationException("Cannot create a timeline from an empty list of snapshots.");

		MovementSnapshots = movementSnapshots;
		SoundSnapshots = soundSnapshots;
	}

	public IReadOnlyList<PlayerMovementSnapshot> MovementSnapshots { get; }
	public IReadOnlyList<SoundSnapshot> SoundSnapshots { get; }

	public PlayerMovementSnapshot GetPlayerMovementSnapshot(int tick)
	{
		tick = Math.Clamp(tick, 0, MovementSnapshots.Count - 1);
		return MovementSnapshots[tick];
	}

	public SoundSnapshot[] GetSoundSnapshots(int tick)
	{
		tick = Math.Clamp(tick, 0, MovementSnapshots.Count - 1);
		return SoundSnapshots.Where(s => s.Tick == tick).ToArray();
	}
}
