namespace DevilDaggersInfo.Core.Replay.Events;

public interface IInputsEvent : IEvent
{
	public bool Left { get; }

	public bool Right { get; }

	public bool Forward { get; }

	public bool Backward { get; }

	public JumpType Jump { get; }

	public bool Shoot { get; }

	public bool ShootHoming { get; }

	public short MouseX { get; }

	public short MouseY { get; }
}
