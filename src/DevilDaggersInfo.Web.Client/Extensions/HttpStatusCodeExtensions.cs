using DevilDaggersInfo.Web.Client.Components.Admin;
using DevilDaggersInfo.Web.Client.Enums;
using System.Net;

namespace DevilDaggersInfo.Web.Client.Extensions;

internal static class HttpStatusCodeExtensions
{
	extension(HttpStatusCode httpStatusCode)
	{
		public ErrorState GetErrorState()
		{
			return httpStatusCode switch
			{
				HttpStatusCode.BadRequest or HttpStatusCode.NotFound => ErrorState.ValidationError,
				_ => ErrorState.FatalError,
			};
		}

		public DeleteState GetDeleteState()
		{
			return httpStatusCode switch
			{
				HttpStatusCode.BadRequest or HttpStatusCode.NotFound => DeleteState.ValidationError,
				_ => DeleteState.FatalError,
			};
		}
	}
}
