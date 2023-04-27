using DevilDaggersInfo.Core.Replay.Events.Enums;

namespace DevilDaggersInfo.Core.Replay.Events.Interfaces;

public interface IInputsEvent : IEvent
{
	bool Left { get; }

	bool Right { get; }

	bool Forward { get; }

	bool Backward { get; }

	JumpType Jump { get; }

	ShootType Shoot { get; }

	ShootType ShootHoming { get; }

	short MouseX { get; }

	short MouseY { get; }
}
