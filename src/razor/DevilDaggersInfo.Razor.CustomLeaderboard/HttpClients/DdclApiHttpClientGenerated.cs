#pragma warning disable CS0105, CS1591, CS8618, RCS1214, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649
using DevilDaggersInfo.Api.Ddcl;
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Api.Ddcl.ProcessMemory;
using DevilDaggersInfo.Api.Ddcl.Spawnsets;
using DevilDaggersInfo.Api.Ddcl.Tools;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.HttpClients;

public partial class DdclApiHttpClient
{
	public async Task<GetCustomEntryReplayBuffer> GetCustomEntryReplayBufferById(int id)
	{
		return await SendGetRequest<GetCustomEntryReplayBuffer>($"api/ddcl/custom-entries/{id}/replay-buffer");
	}

	public async Task<HttpResponseMessage> SubmitScoreForDdclObsolete(AddUploadRequest uploadRequest)
	{
		return await SendRequest(new HttpMethod("POST"), $"/api/custom-entries/submit", JsonContent.Create(uploadRequest));
	}

	public async Task<HttpResponseMessage> SubmitScoreForDdcl(AddUploadRequest uploadRequest)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/ddcl/custom-entries/submit", JsonContent.Create(uploadRequest));
	}

	public async Task<GetCustomLeaderboard> GetCustomLeaderboardBySpawnsetHash(byte[] hash)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(hash), Uri.EscapeDataString(Convert.ToBase64String(hash)) }
		};
		return await SendGetRequest<GetCustomLeaderboard>(BuildUrlWithQuery($"api/ddcl/custom-leaderboards/by-hash", queryParameters));
	}

	public async Task<Page<GetCustomLeaderboardForOverview>> GetCustomLeaderboardOverview(CustomLeaderboardCategory category, int pageIndex, int pageSize, int selectedPlayerId, bool onlyFeatured)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(category), category },
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(selectedPlayerId), selectedPlayerId },
			{ nameof(onlyFeatured), onlyFeatured }
		};
		return await SendGetRequest<Page<GetCustomLeaderboardForOverview>>(BuildUrlWithQuery($"api/ddcl/custom-leaderboards/overview", queryParameters));
	}

	public async Task<HttpResponseMessage> CustomLeaderboardExistsBySpawnsetHash(byte[] hash)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(hash), Uri.EscapeDataString(Convert.ToBase64String(hash)) }
		};
		return await SendRequest(new HttpMethod("HEAD"), BuildUrlWithQuery($"api/ddcl/custom-leaderboards/exists", queryParameters));
	}

	public async Task<HttpResponseMessage> CustomLeaderboardExistsBySpawnsetHashObsolete(byte[] hash)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(hash), Uri.EscapeDataString(Convert.ToBase64String(hash)) }
		};
		return await SendRequest(new HttpMethod("HEAD"), BuildUrlWithQuery($"/api/custom-leaderboards", queryParameters));
	}

	public async Task<GetMarker> GetMarkerObsolete(SupportedOperatingSystem operatingSystem)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(operatingSystem), operatingSystem }
		};
		return await SendGetRequest<GetMarker>(BuildUrlWithQuery($"/api/process-memory/marker", queryParameters));
	}

	public async Task<GetMarker> GetMarker(SupportedOperatingSystem operatingSystem)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(operatingSystem), operatingSystem }
		};
		return await SendGetRequest<GetMarker>(BuildUrlWithQuery($"api/ddcl/process-memory/marker", queryParameters));
	}

	public async Task<GetSpawnsetBuffer> GetSpawnsetBufferById(int id)
	{
		return await SendGetRequest<GetSpawnsetBuffer>($"api/ddcl/spawnsets/{id}/buffer");
	}

	public async Task<GetUpdate> GetUpdates(ToolPublishMethod publishMethod, ToolBuildType buildType)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(publishMethod), publishMethod },
			{ nameof(buildType), buildType }
		};
		return await SendGetRequest<GetUpdate>(BuildUrlWithQuery($"api/ddcl/updates/", queryParameters));
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
