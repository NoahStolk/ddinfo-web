namespace DevilDaggersInfo.Core.Mod.Exceptions;

[Serializable]
public class InvalidObjException : Exception
{
	public InvalidObjException()
	{
	}

	public InvalidObjException(string? message)
		: base(message)
	{
	}

	public InvalidObjException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected InvalidObjException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
