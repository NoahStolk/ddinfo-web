using Blazored.LocalStorage;
using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.BackgroundServices;
using DevilDaggersInfo.Web.ApiSpec.Admin.Caches;
using DevilDaggersInfo.Web.ApiSpec.Admin.CustomEntries;
using DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.ApiSpec.Admin.Database;
using DevilDaggersInfo.Web.ApiSpec.Admin.Donations;
using DevilDaggersInfo.Web.ApiSpec.Admin.FileSystem;
using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;
using DevilDaggersInfo.Web.ApiSpec.Admin.Players;
using DevilDaggersInfo.Web.ApiSpec.Admin.Spawnsets;
using DevilDaggersInfo.Web.ApiSpec.Admin.Users;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.Client.HttpClients;

public class AdminApiHttpClient : ApiHttpClient
{
	public AdminApiHttpClient(HttpClient client, ILocalStorageService localStorageService)
		: base(client, localStorageService)
	{
	}

	public async Task<List<GetBackgroundServiceEntry>> GetBackgroundServices()
	{
		return await SendGetRequest<List<GetBackgroundServiceEntry>>("api/admin/background-services/");
	}

	public async Task<List<GetCacheEntry>> GetCaches()
	{
		return await SendGetRequest<List<GetCacheEntry>>("api/admin/cache/");
	}

	public async Task<HttpResponseMessage> ClearCache(string cacheType)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/cache/clear-cache", JsonContent.Create(cacheType));
	}

	public async Task<List<GetCustomEntryForOverview>> GetCustomEntries()
	{
		return await SendGetRequest<List<GetCustomEntryForOverview>>("api/admin/custom-entries/");
	}

	public async Task<GetCustomEntry> GetCustomEntryById(int id)
	{
		return await SendGetRequest<GetCustomEntry>($"api/admin/custom-entries/{id}");
	}

	public async Task<HttpResponseMessage> AddCustomEntry(AddCustomEntry addCustomEntry)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/custom-entries/", JsonContent.Create(addCustomEntry));
	}

	public async Task<HttpResponseMessage> EditCustomEntryById(int id, EditCustomEntry editCustomEntry)
	{
		return await SendRequest(HttpMethod.Put, $"api/admin/custom-entries/{id}", JsonContent.Create(editCustomEntry));
	}

	public async Task<HttpResponseMessage> DeleteCustomEntryById(int id)
	{
		return await SendRequest(HttpMethod.Delete, $"api/admin/custom-entries/{id}");
	}

	public async Task<Page<GetCustomLeaderboardForOverview>> GetCustomLeaderboards(string? filter, int pageIndex, int pageSize, CustomLeaderboardSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(filter), filter },
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending },
		};
		return await SendGetRequest<Page<GetCustomLeaderboardForOverview>>(BuildUrlWithQuery("api/admin/custom-leaderboards/", queryParameters));
	}

	public async Task<GetCustomLeaderboard> GetCustomLeaderboardById(int id)
	{
		return await SendGetRequest<GetCustomLeaderboard>($"api/admin/custom-leaderboards/{id}");
	}

	public async Task<HttpResponseMessage> AddCustomLeaderboard(AddCustomLeaderboard addCustomLeaderboard)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/custom-leaderboards/", JsonContent.Create(addCustomLeaderboard));
	}

	public async Task<HttpResponseMessage> EditCustomLeaderboardById(int id, EditCustomLeaderboard editCustomLeaderboard)
	{
		return await SendRequest(HttpMethod.Put, $"api/admin/custom-leaderboards/{id}", JsonContent.Create(editCustomLeaderboard));
	}

	public async Task<HttpResponseMessage> DeleteCustomLeaderboardById(int id)
	{
		return await SendRequest(HttpMethod.Delete, $"api/admin/custom-leaderboards/{id}");
	}

	public async Task<List<GetDatabaseTableEntry>> GetDatabaseInfo()
	{
		return await SendGetRequest<List<GetDatabaseTableEntry>>("api/admin/database/");
	}

	public async Task<Page<GetDonationForOverview>> GetDonations(string? filter, int pageIndex, int pageSize, DonationSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(filter), filter },
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending },
		};
		return await SendGetRequest<Page<GetDonationForOverview>>(BuildUrlWithQuery("api/admin/donations/", queryParameters));
	}

	public async Task<GetDonation> GetDonationById(int id)
	{
		return await SendGetRequest<GetDonation>($"api/admin/donations/{id}");
	}

	public async Task<HttpResponseMessage> AddDonation(AddDonation addDonation)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/donations/", JsonContent.Create(addDonation));
	}

	public async Task<HttpResponseMessage> EditDonationById(int id, EditDonation editDonation)
	{
		return await SendRequest(HttpMethod.Put, $"api/admin/donations/{id}", JsonContent.Create(editDonation));
	}

	public async Task<HttpResponseMessage> DeleteDonationById(int id)
	{
		return await SendRequest(HttpMethod.Delete, $"api/admin/donations/{id}");
	}

	public async Task<List<GetFileSystemEntry>> GetFileSystemInfo()
	{
		return await SendGetRequest<List<GetFileSystemEntry>>("api/admin/file-system/");
	}

	public async Task<HttpResponseMessage> TestException(string? message)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/logging/test-exception", JsonContent.Create(message));
	}

	public async Task<HttpResponseMessage> LogError(string? message)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/logging/log-error", JsonContent.Create(message));
	}

	public async Task<HttpResponseMessage> LogWarning(string? message)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/logging/log-warning", JsonContent.Create(message));
	}

	public async Task<HttpResponseMessage> LogInfo(string? message)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/logging/log-info", JsonContent.Create(message));
	}

	public async Task<List<string>> GetMarkers()
	{
		return await SendGetRequest<List<string>>("api/admin/markers/");
	}

	public async Task<HttpResponseMessage> EditMarker(string name, long value)
	{
		return await SendRequest(HttpMethod.Put, $"api/admin/markers/{name}", JsonContent.Create(value));
	}

	public async Task<Page<GetModForOverview>> GetMods(string? filter, int pageIndex, int pageSize, ModSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(filter), filter },
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending },
		};
		return await SendGetRequest<Page<GetModForOverview>>(BuildUrlWithQuery("api/admin/mods/", queryParameters));
	}

	public async Task<List<GetModName>> GetModNames()
	{
		return await SendGetRequest<List<GetModName>>("api/admin/mods/names");
	}

	public async Task<GetMod> GetModById(int id)
	{
		return await SendGetRequest<GetMod>($"api/admin/mods/{id}");
	}

	public async Task<HttpResponseMessage> AddMod(AddMod addMod)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/mods/", JsonContent.Create(addMod));
	}

	public async Task<HttpResponseMessage> EditModById(int id, EditMod editMod)
	{
		return await SendRequest(HttpMethod.Put, $"api/admin/mods/{id}", JsonContent.Create(editMod));
	}

	public async Task<HttpResponseMessage> DeleteModById(int id)
	{
		return await SendRequest(HttpMethod.Delete, $"api/admin/mods/{id}");
	}

	public async Task<Page<GetPlayerForOverview>> GetPlayers(string? filter, int pageIndex, int pageSize, PlayerSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(filter), filter },
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending },
		};
		return await SendGetRequest<Page<GetPlayerForOverview>>(BuildUrlWithQuery("api/admin/players/", queryParameters));
	}

	public async Task<List<GetPlayerName>> GetPlayerNames()
	{
		return await SendGetRequest<List<GetPlayerName>>("api/admin/players/names");
	}

	public async Task<GetPlayer> GetPlayerById(int id)
	{
		return await SendGetRequest<GetPlayer>($"api/admin/players/{id}");
	}

	public async Task<HttpResponseMessage> AddPlayer(AddPlayer addPlayer)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/players/", JsonContent.Create(addPlayer));
	}

	public async Task<HttpResponseMessage> EditPlayerById(int id, EditPlayer editPlayer)
	{
		return await SendRequest(HttpMethod.Put, $"api/admin/players/{id}", JsonContent.Create(editPlayer));
	}

	public async Task<HttpResponseMessage> DeletePlayerById(int id)
	{
		return await SendRequest(HttpMethod.Delete, $"api/admin/players/{id}");
	}

	public async Task<Page<GetSpawnsetForOverview>> GetSpawnsets(string? filter, int pageIndex, int pageSize, SpawnsetSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(filter), filter },
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending },
		};
		return await SendGetRequest<Page<GetSpawnsetForOverview>>(BuildUrlWithQuery("api/admin/spawnsets/", queryParameters));
	}

	public async Task<List<GetSpawnsetName>> GetSpawnsetNames()
	{
		return await SendGetRequest<List<GetSpawnsetName>>("api/admin/spawnsets/names");
	}

	public async Task<GetSpawnset> GetSpawnsetById(int id)
	{
		return await SendGetRequest<GetSpawnset>($"api/admin/spawnsets/{id}");
	}

	public async Task<HttpResponseMessage> AddSpawnset(AddSpawnset addSpawnset)
	{
		return await SendRequest(HttpMethod.Post, "api/admin/spawnsets/", JsonContent.Create(addSpawnset));
	}

	public async Task<HttpResponseMessage> EditSpawnsetById(int id, EditSpawnset editSpawnset)
	{
		return await SendRequest(HttpMethod.Put, $"api/admin/spawnsets/{id}", JsonContent.Create(editSpawnset));
	}

	public async Task<HttpResponseMessage> DeleteSpawnsetById(int id)
	{
		return await SendRequest(HttpMethod.Delete, $"api/admin/spawnsets/{id}");
	}

	public async Task<List<GetUser>> GetUsers()
	{
		return await SendGetRequest<List<GetUser>>("api/admin/users/");
	}

	public async Task<GetUser> GetUserById(int id)
	{
		return await SendGetRequest<GetUser>($"api/admin/users/{id}");
	}

	public async Task<HttpResponseMessage> ToggleRole(int id, ToggleRole toggleRole)
	{
		return await SendRequest(HttpMethod.Patch, $"api/admin/users/{id}/toggle-role", JsonContent.Create(toggleRole));
	}

	public async Task<HttpResponseMessage> AssignPlayer(int id, AssignPlayer assignPlayer)
	{
		return await SendRequest(HttpMethod.Put, $"api/admin/users/{id}/assign-player", JsonContent.Create(assignPlayer));
	}

	public async Task<HttpResponseMessage> ResetPasswordForUserById(int id, ResetPassword resetPassword)
	{
		return await SendRequest(HttpMethod.Put, $"api/admin/users/{id}/reset-password", JsonContent.Create(resetPassword));
	}

	public async Task<HttpResponseMessage> DeleteUserById(int id)
	{
		return await SendRequest(HttpMethod.Delete, $"api/admin/users/{id}");
	}
}
