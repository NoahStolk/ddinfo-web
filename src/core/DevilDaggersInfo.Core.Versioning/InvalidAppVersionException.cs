using System.Runtime.Serialization;

namespace DevilDaggersInfo.Core.Versioning;

[Serializable]
public class InvalidAppVersionException : Exception
{
	public InvalidAppVersionException()
	{
	}

	public InvalidAppVersionException(string? message)
		: base(message)
	{
	}

	public InvalidAppVersionException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected InvalidAppVersionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
