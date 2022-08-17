using System.Diagnostics.CodeAnalysis;

namespace DevilDaggersInfo.Core.Replay;

public interface IReplayBinaryHeader<out TSelf>
	where TSelf : IReplayBinaryHeader<TSelf>
{
	/// <summary>
	/// Indicates if the events following this header structure are prefixed with 32-bit integer length.
	/// </summary>
	static abstract bool UsesLengthPrefixedEvents { get; }

	static abstract TSelf CreateFromBinaryReader(BinaryReader br);

	static abstract bool IdentifierIsValid(BinaryReader br, [MaybeNullWhen(false)] out byte[]? identifier);

	static abstract TSelf CreateDefault();

	byte[] ToBytes();
}
