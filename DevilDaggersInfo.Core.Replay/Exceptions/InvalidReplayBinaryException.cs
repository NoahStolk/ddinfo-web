using System.Runtime.Serialization;

namespace DevilDaggersInfo.Core.Replay.Exceptions;

[Serializable]
public class InvalidReplayBinaryException : Exception
{
	public InvalidReplayBinaryException()
	{
	}

	public InvalidReplayBinaryException(string? message)
		: base(message)
	{
	}

	public InvalidReplayBinaryException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected InvalidReplayBinaryException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
