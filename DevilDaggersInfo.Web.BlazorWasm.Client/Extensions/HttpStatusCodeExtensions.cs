namespace DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;

public static class HttpStatusCodeExtensions
{
	public static bool IsUserError(this HttpStatusCode httpStatusCode)
		=> (int)httpStatusCode is >= 400 and < 500;

	public static bool IsUserError(this HttpStatusCode? httpStatusCode)
		=> httpStatusCode.HasValue && IsUserError(httpStatusCode.Value);
}
