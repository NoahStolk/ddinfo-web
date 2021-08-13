using System;
using System.Runtime.Serialization;

namespace DevilDaggersWebsite.BlazorWasm.Server.Exceptions
{
	[Serializable]
	public class InvalidModArchiveException : Exception
	{
		public InvalidModArchiveException()
		{
		}

		public InvalidModArchiveException(string? message)
			: base(message)
		{
		}

		public InvalidModArchiveException(string? message, Exception? innerException)
			: base(message, innerException)
		{
		}

		protected InvalidModArchiveException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
