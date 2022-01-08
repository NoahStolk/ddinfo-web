#pragma warning disable CS0105, CS1591, CS8618, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Assets;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Donations;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Integrations;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistoryStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Leaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.ProcessMemory;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.WorldRecords;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public partial class PublicApiHttpClient
{
	public async Task<Dictionary<string, List<GetAssetInfo>>> GetAssetInfoForDdae()
	{
		return await SendGetRequest<Dictionary<string, List<GetAssetInfo>>>($"api/assets/ddae/info");
	}

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

	public async Task<Task> GetCustomEntryReplayById(int id)
	{
		return await SendGetRequest<Task>($"api/custom-entries/{id}/replay");
	}

	public async Task<GetCustomEntryData> GetCustomEntryDataById(int id)
	{
		return await SendGetRequest<GetCustomEntryData>($"api/custom-entries/{id}/data");
	}

	public async Task<List<GetCustomLeaderboardStatisticsForPlayer>> GetCustomLeaderboardStatisticsByPlayerId(int playerId)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(playerId), playerId }
		};
		return await SendGetRequest<List<GetCustomLeaderboardStatisticsForPlayer>>(UrlBuilderUtils.BuildUrlWithQuery($"api/custom-entries/player-stats", queryParameters));
	}

	public async Task<HttpResponseMessage> SubmitScoreForDdcl(AddUploadRequest uploadRequest)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/custom-entries/submit", JsonContent.Create(uploadRequest));
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
		return await SendGetRequest<Page<GetCustomLeaderboardOverview>>(UrlBuilderUtils.BuildUrlWithQuery($"api/custom-leaderboards/", queryParameters));
	}

	public async Task<List<GetCustomLeaderboardDdLive>> GetCustomLeaderboardsDdLive()
	{
		return await SendGetRequest<List<GetCustomLeaderboardDdLive>>($"api/custom-leaderboards/ddlive");
	}

	public async Task<GetTotalCustomLeaderboardData> GetTotalCustomLeaderboardData()
	{
		return await SendGetRequest<GetTotalCustomLeaderboardData>($"api/custom-leaderboards/total-data");
	}

	public async Task<GetNumberOfCustomLeaderboards> GetNumberOfCustomLeaderboards()
	{
		return await SendGetRequest<GetNumberOfCustomLeaderboards>($"api/custom-leaderboards/number-of-custom-leaderboards");
	}

	public async Task<GetCustomLeaderboard> GetCustomLeaderboardById(int id)
	{
		return await SendGetRequest<GetCustomLeaderboard>($"api/custom-leaderboards/{id}");
	}

	public async Task<List<GetDonator>> GetDonators()
	{
		return await SendGetRequest<List<GetDonator>>($"api/donations/donators");
	}

	public async Task<DdstatsRustAccessInfo> GetDdstatsRustAccessInfo()
	{
		return await SendGetRequest<DdstatsRustAccessInfo>($"api/integrations/ddstats-rust");
	}

	public async Task<GetLeaderboardHistory> GetLeaderboardHistory(DateTime dateTime)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(dateTime), dateTime }
		};
		return await SendGetRequest<GetLeaderboardHistory>(UrlBuilderUtils.BuildUrlWithQuery($"api/leaderboard-history/", queryParameters));
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
		return await SendGetRequest<GetLeaderboard?>(UrlBuilderUtils.BuildUrlWithQuery($"api/leaderboards/", queryParameters));
	}

	public async Task<GetEntry> GetEntryById(int id)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(id), id }
		};
		return await SendGetRequest<GetEntry>(UrlBuilderUtils.BuildUrlWithQuery($"api/leaderboards/entry/by-id", queryParameters));
	}

	public async Task<List<GetEntry>> GetEntriesByIds(string commaSeparatedIds)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(commaSeparatedIds), commaSeparatedIds }
		};
		return await SendGetRequest<List<GetEntry>>(UrlBuilderUtils.BuildUrlWithQuery($"api/leaderboards/entry/by-ids", queryParameters));
	}

	public async Task<List<GetEntry>> GetEntriesByName(string name)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(name), name }
		};
		return await SendGetRequest<List<GetEntry>>(UrlBuilderUtils.BuildUrlWithQuery($"api/leaderboards/entry/by-username", queryParameters));
	}

	public async Task<GetEntry> GetEntryByRank(int rank)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(rank), rank }
		};
		return await SendGetRequest<GetEntry>(UrlBuilderUtils.BuildUrlWithQuery($"api/leaderboards/entry/by-rank", queryParameters));
	}

	public async Task<GetLeaderboardStatistics> GetLeaderboardStatistics()
	{
		return await SendGetRequest<GetLeaderboardStatistics>($"api/leaderboard-statistics/");
	}

	public async Task<GetLeaderboardStatisticsDdLive> GetLeaderboardStatisticsDdLive(LeaderboardStatisticsLimitDdLive top)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(top), top }
		};
		return await SendGetRequest<GetLeaderboardStatisticsDdLive>(UrlBuilderUtils.BuildUrlWithQuery($"api/leaderboard-statistics/ddlive", queryParameters));
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
		return await SendGetRequest<Page<GetModOverview>>(UrlBuilderUtils.BuildUrlWithQuery($"api/mods/", queryParameters));
	}

	public async Task<List<GetModDdae>> GetModsForDdae(string? authorFilter, string? nameFilter, bool? isHostedFilter)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(authorFilter), authorFilter },
			{ nameof(nameFilter), nameFilter },
			{ nameof(isHostedFilter), isHostedFilter }
		};
		return await SendGetRequest<List<GetModDdae>>(UrlBuilderUtils.BuildUrlWithQuery($"api/mods/ddae", queryParameters));
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
		return await SendGetRequest<List<GetModName>>(UrlBuilderUtils.BuildUrlWithQuery($"api/mods/by-author", queryParameters));
	}

	public async Task<Task> GetScreenshotByFilePath(string modName, string fileName)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(modName), modName },
			{ nameof(fileName), fileName }
		};
		return await SendGetRequest<Task>(UrlBuilderUtils.BuildUrlWithQuery($"api/mod-screenshots/", queryParameters));
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

	public async Task<string> GetPlayerFlagById(int id)
	{
		return await SendGetRequest<string>($"api/players/{id}/flag");
	}

	public async Task<GetPlayerHistory> GetPlayerHistoryById(int id)
	{
		return await SendGetRequest<GetPlayerHistory>($"api/players/{id}/history");
	}

	public async Task<Marker> GetMarker(SupportedOperatingSystem operatingSystem)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(operatingSystem), operatingSystem }
		};
		return await SendGetRequest<Marker>(UrlBuilderUtils.BuildUrlWithQuery($"api/process-memory/marker", queryParameters));
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
		return await SendGetRequest<Page<GetSpawnsetOverview>>(UrlBuilderUtils.BuildUrlWithQuery($"api/spawnsets/", queryParameters));
	}

	public async Task<List<GetSpawnsetDdse>> GetSpawnsetsForDdse(string? authorFilter, string? nameFilter)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(authorFilter), authorFilter },
			{ nameof(nameFilter), nameFilter }
		};
		return await SendGetRequest<List<GetSpawnsetDdse>>(UrlBuilderUtils.BuildUrlWithQuery($"api/spawnsets/ddse", queryParameters));
	}

	public async Task<GetSpawnsetByHash> GetSpawnsetByHash(byte[] hash)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(hash), hash }
		};
		return await SendGetRequest<GetSpawnsetByHash>(UrlBuilderUtils.BuildUrlWithQuery($"api/spawnsets/by-hash", queryParameters));
	}

	public async Task<GetSpawnsetNameByHash> GetSpawnsetNameByHash(byte[] hash)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(hash), hash }
		};
		return await SendGetRequest<GetSpawnsetNameByHash>(UrlBuilderUtils.BuildUrlWithQuery($"api/spawnsets/name-by-hash", queryParameters));
	}

	public async Task<byte[]> GetSpawnsetHash(string fileName)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(fileName), fileName }
		};
		return await SendGetRequest<byte[]>(UrlBuilderUtils.BuildUrlWithQuery($"api/spawnsets/hash", queryParameters));
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
		return await SendGetRequest<byte[]>(UrlBuilderUtils.BuildUrlWithQuery($"api/spawnsets/default", queryParameters));
	}

	public async Task<List<GetSpawnsetName>> GetSpawnsetsByAuthorId(int playerId)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(playerId), playerId }
		};
		return await SendGetRequest<List<GetSpawnsetName>>(UrlBuilderUtils.BuildUrlWithQuery($"api/spawnsets/by-author", queryParameters));
	}

	public async Task<GetTool> GetTool(string toolName)
	{
		return await SendGetRequest<GetTool>($"api/tools/{toolName}");
	}

	public async Task<Task> GetToolFile(string toolName)
	{
		return await SendGetRequest<Task>($"api/tools/{toolName}/file");
	}

	public async Task<GetWorldRecordDataContainer> GetWorldRecordData()
	{
		return await SendGetRequest<GetWorldRecordDataContainer>($"api/world-records/");
	}

}

#pragma warning restore CS0105, CS1591, CS8618, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649
