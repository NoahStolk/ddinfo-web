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

	public Vector3 GetPositionAtTime(float time)
	{
		if (time < First.Time)
			return First.Position;

		if (time > Last.Time)
			return Last.Position;

		PlayerMovementSnapshot a = Snapshots.LastOrDefault(s => s.Time < time) ?? First;
		PlayerMovementSnapshot b = Snapshots.FirstOrDefault(s => s.Time > time) ?? Last;

		return Vector3.Lerp(a.Position, b.Position, (time - a.Time) / (b.Time - a.Time));
	}
}
