namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class UrlBuilderUtils
{
	public static string BuildUrlWithQuery(this string baseUrl, Dictionary<string, object?> queryParameters)
	{
		if (queryParameters.Count == 0)
			return baseUrl;

		string queryParameterString = string.Join('&', queryParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
		return $"{baseUrl}?{queryParameterString}";
	}
}
