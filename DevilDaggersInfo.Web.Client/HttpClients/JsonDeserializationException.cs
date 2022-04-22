using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

[Serializable]
public class JsonDeserializationException : Exception
{
	public JsonDeserializationException()
	{
	}

	public JsonDeserializationException(string? message)
		: base(message)
	{
	}

	public JsonDeserializationException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected JsonDeserializationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
