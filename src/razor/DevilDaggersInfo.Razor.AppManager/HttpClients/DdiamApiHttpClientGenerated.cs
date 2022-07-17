#pragma warning disable CS0105, CS1591, CS8618, RCS1214, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649
using DevilDaggersInfo.Api.Ddiam;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Razor.AppManager.HttpClients;

public partial class DdiamApiHttpClient
{
	public async Task<List<GetApp>> GetApps(OperatingSystemType os)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(os), os }
		};
		return await SendGetRequest<List<GetApp>>(BuildUrlWithQuery($"api/ddiam/apps/", queryParameters));
	}

	private static string BuildUrlWithQuery(string baseUrl, Dictionary<string, object?> queryParameters)
	{
		if (queryParameters.Count == 0)
			return baseUrl;

		string queryParameterString = string.Join('&', queryParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
		return $"{baseUrl.TrimEnd('/')}?{queryParameterString}";
	}
}

#pragma warning restore CS0105, CS1591, CS8618, RCS1214, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649
