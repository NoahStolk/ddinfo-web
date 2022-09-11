using System.Runtime.Serialization;

namespace DevilDaggersInfo.Core.Spawnset.Exceptions;

[Serializable]
public class InvalidSpawnsetBinaryException : Exception
{
	public InvalidSpawnsetBinaryException()
	{
	}

	public InvalidSpawnsetBinaryException(string? message)
		: base(message)
	{
	}

	public InvalidSpawnsetBinaryException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected InvalidSpawnsetBinaryException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
