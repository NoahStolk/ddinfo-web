using System;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Exceptions
{
	[Serializable]
	public class InvalidModBinaryException : Exception
	{
		public InvalidModBinaryException()
		{
		}

		public InvalidModBinaryException(string? message)
			: base(message)
		{
		}

		public InvalidModBinaryException(string? message, Exception? innerException)
			: base(message, innerException)
		{
		}

		protected InvalidModBinaryException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
