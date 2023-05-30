// <auto-generated>
// This code was generated by DevilDaggersInfo.
// </auto-generated>

#pragma warning disable CS0105, CS1591, CS8618, RCS1214, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649

#nullable enable

using DevilDaggersInfo.Api.App;
using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Api.App.ProcessMemory;
using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.Api.App.Updates;
using System.Net.Http.Json;

namespace DevilDaggersInfo.App.Networking;

public partial class AppApiHttpClient
{
	public async Task<GetCustomEntryReplayBuffer> GetCustomEntryReplayBufferById(int id)
	{
		return await SendGetRequest<GetCustomEntryReplayBuffer>($"api/app/custom-entries/{id}/replay-buffer");
	}

	public async Task<HttpResponseMessage> SubmitScore(AddUploadRequest uploadRequest)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/app/custom-entries/submit", JsonContent.Create(uploadRequest));
	}

	public async Task<List<GetCustomLeaderboardForOverview>> GetCustomLeaderboards(int selectedPlayerId)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(selectedPlayerId), selectedPlayerId }
		};
		return await SendGetRequest<List<GetCustomLeaderboardForOverview>>(BuildUrlWithQuery($"api/app/custom-leaderboards/", queryParameters));
	}

	public async Task<GetCustomLeaderboard> GetCustomLeaderboardById(int id)
	{
		return await SendGetRequest<GetCustomLeaderboard>($"api/app/custom-leaderboards/{id}");
	}

	public async Task<GetCustomLeaderboard> GetCustomLeaderboardBySpawnsetHash(byte[] hash)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(hash), Uri.EscapeDataString(Convert.ToBase64String(hash)) }
		};
		return await SendGetRequest<GetCustomLeaderboard>(BuildUrlWithQuery($"api/app/custom-leaderboards/by-hash", queryParameters));
	}

	public async Task<HttpResponseMessage> CustomLeaderboardExistsBySpawnsetHash(byte[] hash)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(hash), Uri.EscapeDataString(Convert.ToBase64String(hash)) }
		};
		return await SendRequest(new HttpMethod("HEAD"), BuildUrlWithQuery($"api/app/custom-leaderboards/exists", queryParameters));
	}

	public async Task<List<GetCustomLeaderboardAllowedCategory>> GetCustomLeaderboardAllowedCategories()
	{
		return await SendGetRequest<List<GetCustomLeaderboardAllowedCategory>>($"api/app/custom-leaderboards/allowed-categories");
	}

	public async Task<GetMarker> GetMarker(AppOperatingSystem appOperatingSystem)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(appOperatingSystem), appOperatingSystem }
		};
		return await SendGetRequest<GetMarker>(BuildUrlWithQuery($"api/app/process-memory/marker", queryParameters));
	}

	public async Task<GetSpawnset> GetSpawnsetById(int id)
	{
		return await SendGetRequest<GetSpawnset>($"api/app/spawnsets/{id}");
	}

	public async Task<GetSpawnsetBuffer> GetSpawnsetBufferById(int id)
	{
		return await SendGetRequest<GetSpawnsetBuffer>($"api/app/spawnsets/{id}/buffer");
	}

	public async Task<GetSpawnsetByHash> GetSpawnsetByHash(byte[] hash)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(hash), Uri.EscapeDataString(Convert.ToBase64String(hash)) }
		};
		return await SendGetRequest<GetSpawnsetByHash>(BuildUrlWithQuery($"api/app/spawnsets/by-hash", queryParameters));
	}

	public async Task<GetLatestVersion> GetLatestVersion(AppOperatingSystem appOperatingSystem)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(appOperatingSystem), appOperatingSystem }
		};
		return await SendGetRequest<GetLatestVersion>(BuildUrlWithQuery($"api/app/updates/latest-version", queryParameters));
	}

	public async Task<Task> GetLatestVersionFile(AppOperatingSystem appOperatingSystem)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(appOperatingSystem), appOperatingSystem }
		};
		return await SendGetRequest<Task>(BuildUrlWithQuery($"api/app/updates/latest-version-file", queryParameters));
	}

	private static string BuildUrlWithQuery(string baseUrl, Dictionary<string, object?> queryParameters)
	{
		if (queryParameters.Count == 0)
			return baseUrl;

		string queryParameterString = string.Join('&', queryParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
		return $"{baseUrl.TrimEnd('/')}?{queryParameterString}";
	}
}

