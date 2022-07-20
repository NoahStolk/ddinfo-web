namespace DevilDaggersInfo.Core.Replay;

public interface IReplayBinaryHeader<TReplayBinaryHeader>
	where TReplayBinaryHeader : IReplayBinaryHeader<TReplayBinaryHeader>
{
	/// <summary>
	/// Indicates if the events following this header structure are prefixed with 32-bit integer length.
	/// </summary>
	static abstract bool UsesLengthPrefixedEvents { get; }

	static abstract TReplayBinaryHeader CreateFromBinaryReader(BinaryReader br);

	static abstract TReplayBinaryHeader CreateDefault();

	byte[] ToBytes();
}
