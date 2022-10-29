namespace DevilDaggersInfo.App.Ui.Base.Exceptions;

public class MissingContentException : Exception
{
	public MissingContentException()
	{
	}

	public MissingContentException(string? message)
		: base(message)
	{
	}

	public MissingContentException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}
}
