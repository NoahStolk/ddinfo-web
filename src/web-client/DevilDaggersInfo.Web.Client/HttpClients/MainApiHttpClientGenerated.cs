#pragma warning disable CS0105, CS1591, CS8618, RCS1214, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649
using DevilDaggersInfo.Api.Main;
using DevilDaggersInfo.Api.Main.Authentication;
using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Api.Main.Donations;
using DevilDaggersInfo.Api.Main.GameVersions;
using DevilDaggersInfo.Api.Main.LeaderboardHistory;
using DevilDaggersInfo.Api.Main.LeaderboardHistoryStatistics;
using DevilDaggersInfo.Api.Main.LeaderboardStatistics;
using DevilDaggersInfo.Api.Main.Leaderboards;
using DevilDaggersInfo.Api.Main.Mods;
using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Api.Main.Spawnsets;
using DevilDaggersInfo.Api.Main.Tools;
using DevilDaggersInfo.Api.Main.WorldRecords;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.Client.HttpClients;

public partial class MainApiHttpClient
{
	public async Task<HttpResponseMessage> Authenticate(AuthenticationRequest authenticationRequest)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/authentication/authenticate", JsonContent.Create(authenticationRequest));
	}

	public async Task<HttpResponseMessage> Login(LoginRequest loginRequest)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/authentication/login", JsonContent.Create(loginRequest));
	}

	public async Task<HttpResponseMessage> Register(RegistrationRequest registrationRequest)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/authentication/register", JsonContent.Create(registrationRequest));
	}

	public async Task<HttpResponseMessage> UpdateName(UpdateNameRequest updateNameRequest)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/authentication/update-name", JsonContent.Create(updateNameRequest));
	}

	public async Task<HttpResponseMessage> UpdatePassword(UpdatePasswordRequest updatePasswordRequest)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/authentication/update-password", JsonContent.Create(updatePasswordRequest));
	}

	public async Task<byte[]> GetCustomEntryReplayBufferById(int id)
	{
		return await SendGetRequest<byte[]>($"api/custom-entries/{id}/replay-buffer");
	}

	public async Task<Task> GetCustomEntryReplayById(int id)
	{
		return await SendGetRequest<Task>($"api/custom-entries/{id}/replay");
	}

	public async Task<GetCustomEntryData> GetCustomEntryDataById(int id)
	{
		return await SendGetRequest<GetCustomEntryData>($"api/custom-entries/{id}/data");
	}

	public async Task<Page<GetCustomLeaderboardOverview>> GetCustomLeaderboards(CustomLeaderboardCategory category, string? spawnsetFilter, string? authorFilter, int pageIndex, int pageSize, CustomLeaderboardSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(category), category },
			{ nameof(spawnsetFilter), spawnsetFilter },
			{ nameof(authorFilter), authorFilter },
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending }
		};
		return await SendGetRequest<Page<GetCustomLeaderboardOverview>>(BuildUrlWithQuery($"api/custom-leaderboards/", queryParameters));
	}

	public async Task<GetGlobalCustomLeaderboard> GetGlobalCustomLeaderboardForCategory(CustomLeaderboardCategory category)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(category), category }
		};
		return await SendGetRequest<GetGlobalCustomLeaderboard>(BuildUrlWithQuery($"api/custom-leaderboards/global-leaderboard", queryParameters));
	}

	public async Task<GetTotalCustomLeaderboardData> GetTotalCustomLeaderboardData()
	{
		return await SendGetRequest<GetTotalCustomLeaderboardData>($"api/custom-leaderboards/total-data");
	}

	public async Task<GetCustomLeaderboard> GetCustomLeaderboardById(int id)
	{
		return await SendGetRequest<GetCustomLeaderboard>($"api/custom-leaderboards/{id}");
	}

	public async Task<List<GetDonator>> GetDonators()
	{
		return await SendGetRequest<List<GetDonator>>($"api/donations/donators");
	}

	public async Task<GetLeaderboardHistory> GetLeaderboardHistory(DateTime dateTime)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(dateTime), dateTime }
		};
		return await SendGetRequest<GetLeaderboardHistory>(BuildUrlWithQuery($"api/leaderboard-history/", queryParameters));
	}

	public async Task<List<GetLeaderboardHistoryStatistics>> GetLeaderboardHistoryStatistics()
	{
		return await SendGetRequest<List<GetLeaderboardHistoryStatistics>>($"api/leaderboard-history-statistics/");
	}

	public async Task<GetLeaderboard?> GetLeaderboard(int rankStart)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(rankStart), rankStart }
		};
		return await SendGetRequest<GetLeaderboard?>(BuildUrlWithQuery($"api/leaderboards/", queryParameters));
	}

	public async Task<GetEntry> GetEntryById(int id)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(id), id }
		};
		return await SendGetRequest<GetEntry>(BuildUrlWithQuery($"api/leaderboards/entry/by-id", queryParameters));
	}

	public async Task<List<GetEntry>> GetEntriesByIds(string commaSeparatedIds)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(commaSeparatedIds), commaSeparatedIds }
		};
		return await SendGetRequest<List<GetEntry>>(BuildUrlWithQuery($"api/leaderboards/entry/by-ids", queryParameters));
	}

	public async Task<List<GetEntry>> GetEntriesByName(string name)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(name), name }
		};
		return await SendGetRequest<List<GetEntry>>(BuildUrlWithQuery($"api/leaderboards/entry/by-username", queryParameters));
	}

	public async Task<GetEntry> GetEntryByRank(int rank)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(rank), rank }
		};
		return await SendGetRequest<GetEntry>(BuildUrlWithQuery($"api/leaderboards/entry/by-rank", queryParameters));
	}

	public async Task<GetLeaderboardStatistics> GetLeaderboardStatistics()
	{
		return await SendGetRequest<GetLeaderboardStatistics>($"api/leaderboard-statistics/");
	}

	public async Task<Page<GetModOverview>> GetMods(bool onlyHosted, string? modFilter, string? authorFilter, int pageIndex, int pageSize, ModSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(onlyHosted), onlyHosted },
			{ nameof(modFilter), modFilter },
			{ nameof(authorFilter), authorFilter },
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending }
		};
		return await SendGetRequest<Page<GetModOverview>>(BuildUrlWithQuery($"api/mods/", queryParameters));
	}

	public async Task<GetMod> GetModById(int id)
	{
		return await SendGetRequest<GetMod>($"api/mods/{id}");
	}

	public async Task<GetTotalModData> GetTotalModData()
	{
		return await SendGetRequest<GetTotalModData>($"api/mods/total-data");
	}

	public async Task<Task> GetModFile(string modName)
	{
		return await SendGetRequest<Task>($"api/mods/{modName}/file");
	}

	public async Task<List<GetModName>> GetModsByAuthorId(int playerId)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(playerId), playerId }
		};
		return await SendGetRequest<List<GetModName>>(BuildUrlWithQuery($"api/mods/by-author", queryParameters));
	}

	public async Task<Task> GetScreenshotByFilePath(string modName, string fileName)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(modName), modName },
			{ nameof(fileName), fileName }
		};
		return await SendGetRequest<Task>(BuildUrlWithQuery($"api/mod-screenshots/", queryParameters));
	}

	public async Task<List<GetPlayerForLeaderboard>> GetPlayersForLeaderboard()
	{
		return await SendGetRequest<List<GetPlayerForLeaderboard>>($"api/players/leaderboard");
	}

	public async Task<List<GetPlayerForSettings>> GetPlayersForSettings()
	{
		return await SendGetRequest<List<GetPlayerForSettings>>($"api/players/settings");
	}

	public async Task<GetPlayer> GetPlayerById(int id)
	{
		return await SendGetRequest<GetPlayer>($"api/players/{id}");
	}

	public async Task<GetPlayerHistory> GetPlayerHistoryById(int id)
	{
		return await SendGetRequest<GetPlayerHistory>($"api/players/{id}/history");
	}

	public async Task<List<GetPlayerCustomLeaderboardStatistics>> GetCustomLeaderboardStatisticsByPlayerId(int id)
	{
		return await SendGetRequest<List<GetPlayerCustomLeaderboardStatistics>>($"api/players/{id}/custom-leaderboard-statistics");
	}

	public async Task<GetPlayerProfile> GetProfileByPlayerId(int id)
	{
		return await SendGetRequest<GetPlayerProfile>($"api/players/{id}/profile");
	}

	public async Task<HttpResponseMessage> UpdateProfileByPlayerId(int id, EditPlayerProfile editPlayerProfile)
	{
		return await SendRequest(new HttpMethod("PUT"), $"api/players/{id}/profile", JsonContent.Create(editPlayerProfile));
	}

	public async Task<Page<GetSpawnsetOverview>> GetSpawnsets(bool practiceOnly, bool withCustomLeaderboardOnly, string? spawnsetFilter, string? authorFilter, int pageIndex, int pageSize, SpawnsetSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(practiceOnly), practiceOnly },
			{ nameof(withCustomLeaderboardOnly), withCustomLeaderboardOnly },
			{ nameof(spawnsetFilter), spawnsetFilter },
			{ nameof(authorFilter), authorFilter },
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending }
		};
		return await SendGetRequest<Page<GetSpawnsetOverview>>(BuildUrlWithQuery($"api/spawnsets/", queryParameters));
	}

	public async Task<GetSpawnsetByHash> GetSpawnsetByHash(byte[] hash)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(hash), Uri.EscapeDataString(Convert.ToBase64String(hash)) }
		};
		return await SendGetRequest<GetSpawnsetByHash>(BuildUrlWithQuery($"api/spawnsets/by-hash", queryParameters));
	}

	public async Task<byte[]> GetSpawnsetHash(string fileName)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(fileName), fileName }
		};
		return await SendGetRequest<byte[]>(BuildUrlWithQuery($"api/spawnsets/hash", queryParameters));
	}

	public async Task<GetTotalSpawnsetData> GetTotalSpawnsetData()
	{
		return await SendGetRequest<GetTotalSpawnsetData>($"api/spawnsets/total-data");
	}

	public async Task<Task> GetSpawnsetFile(string fileName)
	{
		return await SendGetRequest<Task>($"api/spawnsets/{fileName}/file");
	}

	public async Task<GetSpawnset> GetSpawnsetById(int id)
	{
		return await SendGetRequest<GetSpawnset>($"api/spawnsets/{id}");
	}

	public async Task<byte[]> GetDefaultSpawnset(GameVersion gameVersion)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(gameVersion), gameVersion }
		};
		return await SendGetRequest<byte[]>(BuildUrlWithQuery($"api/spawnsets/default", queryParameters));
	}

	public async Task<List<GetSpawnsetName>> GetSpawnsetsByAuthorId(int playerId)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(playerId), playerId }
		};
		return await SendGetRequest<List<GetSpawnsetName>>(BuildUrlWithQuery($"api/spawnsets/by-author", queryParameters));
	}

	public async Task<GetTool> GetTool(string toolName)
	{
		return await SendGetRequest<GetTool>($"api/tools/{toolName}");
	}

	public async Task<Task> GetToolDistributionFile(string toolName, ToolPublishMethod publishMethod, ToolBuildType buildType, string? version)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(publishMethod), publishMethod },
			{ nameof(buildType), buildType },
			{ nameof(version), version }
		};
		return await SendGetRequest<Task>(BuildUrlWithQuery($"api/tools/{toolName}/file", queryParameters));
	}

	public async Task<GetToolDistribution> GetLatestToolDistribution(string toolName, ToolPublishMethod publishMethod, ToolBuildType buildType)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(publishMethod), publishMethod },
			{ nameof(buildType), buildType }
		};
		return await SendGetRequest<GetToolDistribution>(BuildUrlWithQuery($"api/tools/{toolName}/distribution-latest", queryParameters));
	}

	public async Task<GetToolDistribution> GetToolDistributionByVersion(string toolName, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(publishMethod), publishMethod },
			{ nameof(buildType), buildType },
			{ nameof(version), version }
		};
		return await SendGetRequest<GetToolDistribution>(BuildUrlWithQuery($"api/tools/{toolName}/distribution", queryParameters));
	}

	public async Task<GetWorldRecordDataContainer> GetWorldRecordData()
	{
		return await SendGetRequest<GetWorldRecordDataContainer>($"api/world-records/");
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
