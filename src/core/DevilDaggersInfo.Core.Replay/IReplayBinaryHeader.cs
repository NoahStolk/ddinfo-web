namespace DevilDaggersInfo.Core.Replay;

public interface IReplayBinaryHeader<TReplayBinaryHeader>
	where TReplayBinaryHeader : IReplayBinaryHeader<TReplayBinaryHeader>
{
	static abstract TReplayBinaryHeader CreateFromBinaryReader(BinaryReader br);

	static abstract TReplayBinaryHeader CreateDefault();

	byte[] ToBytes();
}
