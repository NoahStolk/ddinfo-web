using System.Runtime.Serialization;

namespace DevilDaggersInfo.Core.CriteriaExpression.Exceptions;

[Serializable]
public class CriteriaExpressionParseException : Exception
{
	public CriteriaExpressionParseException()
	{
	}

	public CriteriaExpressionParseException(string? message)
		: base(message)
	{
	}

	public CriteriaExpressionParseException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected CriteriaExpressionParseException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
