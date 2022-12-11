namespace DevilDaggersInfo.Core.Replay.PostProcessing.PlayerMovement;

public record PlayerMovementTimeline
{
	public PlayerMovementTimeline(IReadOnlyList<PlayerMovementSnapshot> snapshots)
	{
		if (snapshots.Count == 0)
			throw new InvalidOperationException("Cannot create a timeline from an empty list of snapshots.");

		Snapshots = snapshots;
		First = Snapshots[0];
		Last = Snapshots[^1];
	}

	public IReadOnlyList<PlayerMovementSnapshot> Snapshots { get; }

	public PlayerMovementSnapshot First { get; }

	public PlayerMovementSnapshot Last { get; }

	public PlayerMovementSnapshot GetSnapshot(float time)
	{
		if (time < First.Time)
			return First;

		if (time > Last.Time)
			return Last;

		return Snapshots.FirstOrDefault(s => s.Time >= time) ?? Last;
	}
}
