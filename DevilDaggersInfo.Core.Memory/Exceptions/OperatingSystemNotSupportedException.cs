namespace DevilDaggersInfo.Core.Memory.Exceptions;

[Serializable]
public class OperatingSystemNotSupportedException : Exception
{
	public OperatingSystemNotSupportedException()
		: this("The application does not support the current operating system.")
	{
	}

	public OperatingSystemNotSupportedException(string? message)
		: base(message)
	{
	}

	public OperatingSystemNotSupportedException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected OperatingSystemNotSupportedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
