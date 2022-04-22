namespace DevilDaggersInfo.Core.Mod.Exceptions;

[Serializable]
public class InvalidModCompilationException : Exception
{
	public InvalidModCompilationException()
	{
	}

	public InvalidModCompilationException(string? message)
		: base(message)
	{
	}

	public InvalidModCompilationException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected InvalidModCompilationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
