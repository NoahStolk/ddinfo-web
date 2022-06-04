#pragma warning disable CS0105, CS1591, CS8618, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Api.Ddcl.ProcessMemory;
using DevilDaggersInfo.Web.Client.Utils;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.Client.HttpClients;

public partial class DdclApiHttpClient
{
	public async Task<HttpResponseMessage> SubmitScoreForDdcl(AddUploadRequest uploadRequest)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/ddcl/custom-entries//api/custom-entries/submit", JsonContent.Create(uploadRequest));
	}

	public async Task<Marker> GetMarker(SupportedOperatingSystem operatingSystem)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(operatingSystem), operatingSystem }
		};
		return await SendGetRequest<Marker>(UrlBuilderUtils.BuildUrlWithQuery($"api/ddcl/process-memory//api/process-memory/marker", queryParameters));
	}

}

#pragma warning restore CS0105, CS1591, CS8618, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649
