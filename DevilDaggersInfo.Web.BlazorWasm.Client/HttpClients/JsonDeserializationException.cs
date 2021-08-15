using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

[Serializable]
#pragma warning disable RCS1194 // Implement exception constructors.
public class JsonDeserializationException : Exception
#pragma warning restore RCS1194 // Implement exception constructors.
{
	public JsonDeserializationException([CallerMemberName] string? methodName = null)
		: base($"JSON deserialization error in {methodName}.")
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
