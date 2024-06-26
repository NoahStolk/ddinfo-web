using DevilDaggersInfo.Web.Client.Components.Admin;
using DevilDaggersInfo.Web.Client.Enums;
using System.Net;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class HttpStatusCodeExtensions
{
	public static ErrorState GetErrorState(this HttpStatusCode httpStatusCode)
	{
		return httpStatusCode switch
		{
			HttpStatusCode.BadRequest or HttpStatusCode.NotFound => ErrorState.ValidationError,
			_ => ErrorState.FatalError,
		};
	}

	public static DeleteState GetDeleteState(this HttpStatusCode httpStatusCode)
	{
		return httpStatusCode switch
		{
			HttpStatusCode.BadRequest or HttpStatusCode.NotFound => DeleteState.ValidationError,
			_ => DeleteState.FatalError,
		};
	}
}
