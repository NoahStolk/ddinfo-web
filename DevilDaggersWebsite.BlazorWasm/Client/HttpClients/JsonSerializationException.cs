using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace DevilDaggersWebsite.BlazorWasm.Client.HttpClients
{
	[Serializable]
#pragma warning disable RCS1194 // Implement exception constructors.
	public class JsonSerializationException : Exception
#pragma warning restore RCS1194 // Implement exception constructors.
	{
		public JsonSerializationException([CallerMemberName] string? methodName = null)
			: base($"JSON deserialization error in {methodName}.")
		{
		}

		public JsonSerializationException(string? message, Exception? innerException)
			: base(message, innerException)
		{
		}

		protected JsonSerializationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
