namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct InputsEvent(bool Left, bool Right, bool Forward, bool Backward, JumpType Jump, ShootType Shoot, ShootType ShootHoming, short MouseX, short MouseY) : IInputsEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x09);
		bw.Write(Left);
		bw.Write(Right);
		bw.Write(Forward);
		bw.Write(Backward);
		bw.Write((byte)Jump);
		bw.Write((byte)Shoot);
		bw.Write((byte)ShootHoming);
		bw.Write(MouseX);
		bw.Write(MouseY);
		bw.Write((byte)0x0a);
	}
}
