#pragma warning disable CS0105, CS1591, CS8618, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Donations;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Spawnsets;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Users;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Admin;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public partial class AdminApiHttpClient
{
	public async Task<HttpResponseMessage> ClearCache(string cacheType)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/admin/caches/clear", JsonContent.Create(cacheType));
	}

	public async Task<Page<GetCustomEntry>> GetCustomEntries(int pageIndex, int pageSize, CustomEntrySorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending }
		};
		return await SendGetRequest<Page<GetCustomEntry>>(UrlBuilderUtils.BuildUrlWithQuery($"api/admin/custom-entries/", queryParameters));
	}

	public async Task<HttpResponseMessage> AddCustomEntry(AddCustomEntry addCustomEntry)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/admin/custom-entries/", JsonContent.Create(addCustomEntry));
	}

	public async Task<HttpResponseMessage> EditCustomEntryById(int id, EditCustomEntry editCustomEntry)
	{
		return await SendRequest(new HttpMethod("PUT"), $"api/admin/custom-entries/{id}", JsonContent.Create(editCustomEntry));
	}

	public async Task<HttpResponseMessage> DeleteCustomEntryById(int id)
	{
		return await SendRequest(new HttpMethod("DELETE"), $"api/admin/custom-entries/{id}");
	}

	public async Task<Page<GetCustomLeaderboard>> GetCustomLeaderboards(int pageIndex, int pageSize, CustomLeaderboardSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending }
		};
		return await SendGetRequest<Page<GetCustomLeaderboard>>(UrlBuilderUtils.BuildUrlWithQuery($"api/admin/custom-leaderboards/", queryParameters));
	}

	public async Task<GetCustomLeaderboard> GetCustomLeaderboardById(int id)
	{
		return await SendGetRequest<GetCustomLeaderboard>($"api/admin/custom-leaderboards/{id}");
	}

	public async Task<HttpResponseMessage> AddCustomLeaderboard(AddCustomLeaderboard addCustomLeaderboard)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/admin/custom-leaderboards/", JsonContent.Create(addCustomLeaderboard));
	}

	public async Task<HttpResponseMessage> EditCustomLeaderboardById(int id, EditCustomLeaderboard editCustomLeaderboard)
	{
		return await SendRequest(new HttpMethod("PUT"), $"api/admin/custom-leaderboards/{id}", JsonContent.Create(editCustomLeaderboard));
	}

	public async Task<HttpResponseMessage> DeleteCustomLeaderboardById(int id)
	{
		return await SendRequest(new HttpMethod("DELETE"), $"api/admin/custom-leaderboards/{id}");
	}

	public async Task<Page<GetDonationForOverview>> GetDonations(int pageIndex, int pageSize, DonationSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending }
		};
		return await SendGetRequest<Page<GetDonationForOverview>>(UrlBuilderUtils.BuildUrlWithQuery($"api/admin/donations/", queryParameters));
	}

	public async Task<GetDonation> GetDonationById(int id)
	{
		return await SendGetRequest<GetDonation>($"api/admin/donations/{id}");
	}

	public async Task<HttpResponseMessage> AddDonation(AddDonation addDonation)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/admin/donations/", JsonContent.Create(addDonation));
	}

	public async Task<HttpResponseMessage> EditDonationById(int id, EditDonation editDonation)
	{
		return await SendRequest(new HttpMethod("PUT"), $"api/admin/donations/{id}", JsonContent.Create(editDonation));
	}

	public async Task<HttpResponseMessage> DeleteDonationById(int id)
	{
		return await SendRequest(new HttpMethod("DELETE"), $"api/admin/donations/{id}");
	}

	public async Task<Page<GetModForOverview>> GetMods(int pageIndex, int pageSize, ModSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending }
		};
		return await SendGetRequest<Page<GetModForOverview>>(UrlBuilderUtils.BuildUrlWithQuery($"api/admin/mods/", queryParameters));
	}

	public async Task<List<GetModName>> GetModNames()
	{
		return await SendGetRequest<List<GetModName>>($"api/admin/mods/names");
	}

	public async Task<GetMod> GetModById(int id)
	{
		return await SendGetRequest<GetMod>($"api/admin/mods/{id}");
	}

	public async Task<HttpResponseMessage> AddMod(AddMod addMod)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/admin/mods/", JsonContent.Create(addMod));
	}

	public async Task<HttpResponseMessage> EditModById(int id, EditMod editMod)
	{
		return await SendRequest(new HttpMethod("PUT"), $"api/admin/mods/{id}", JsonContent.Create(editMod));
	}

	public async Task<HttpResponseMessage> DeleteModById(int id)
	{
		return await SendRequest(new HttpMethod("DELETE"), $"api/admin/mods/{id}");
	}

	public async Task<Page<GetPlayerForOverview>> GetPlayers(int pageIndex, int pageSize, PlayerSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending }
		};
		return await SendGetRequest<Page<GetPlayerForOverview>>(UrlBuilderUtils.BuildUrlWithQuery($"api/admin/players/", queryParameters));
	}

	public async Task<List<GetPlayerName>> GetPlayerNames()
	{
		return await SendGetRequest<List<GetPlayerName>>($"api/admin/players/names");
	}

	public async Task<GetPlayer> GetPlayerById(int id)
	{
		return await SendGetRequest<GetPlayer>($"api/admin/players/{id}");
	}

	public async Task<HttpResponseMessage> AddPlayer(AddPlayer addPlayer)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/admin/players/", JsonContent.Create(addPlayer));
	}

	public async Task<HttpResponseMessage> EditPlayerById(int id, EditPlayer editPlayer)
	{
		return await SendRequest(new HttpMethod("PUT"), $"api/admin/players/{id}", JsonContent.Create(editPlayer));
	}

	public async Task<HttpResponseMessage> DeletePlayerById(int id)
	{
		return await SendRequest(new HttpMethod("DELETE"), $"api/admin/players/{id}");
	}

	public async Task<Page<GetSpawnsetForOverview>> GetSpawnsets(int pageIndex, int pageSize, SpawnsetSorting? sortBy, bool ascending)
	{
		Dictionary<string, object?> queryParameters = new()
		{
			{ nameof(pageIndex), pageIndex },
			{ nameof(pageSize), pageSize },
			{ nameof(sortBy), sortBy },
			{ nameof(ascending), ascending }
		};
		return await SendGetRequest<Page<GetSpawnsetForOverview>>(UrlBuilderUtils.BuildUrlWithQuery($"api/admin/spawnsets/", queryParameters));
	}

	public async Task<List<GetSpawnsetName>> GetSpawnsetNames()
	{
		return await SendGetRequest<List<GetSpawnsetName>>($"api/admin/spawnsets/names");
	}

	public async Task<GetSpawnset> GetSpawnsetById(int id)
	{
		return await SendGetRequest<GetSpawnset>($"api/admin/spawnsets/{id}");
	}

	public async Task<HttpResponseMessage> AddSpawnset(AddSpawnset addSpawnset)
	{
		return await SendRequest(new HttpMethod("POST"), $"api/admin/spawnsets/", JsonContent.Create(addSpawnset));
	}

	public async Task<HttpResponseMessage> EditSpawnsetById(int id, EditSpawnset editSpawnset)
	{
		return await SendRequest(new HttpMethod("PUT"), $"api/admin/spawnsets/{id}", JsonContent.Create(editSpawnset));
	}

	public async Task<HttpResponseMessage> DeleteSpawnsetById(int id)
	{
		return await SendRequest(new HttpMethod("DELETE"), $"api/admin/spawnsets/{id}");
	}

	public async Task<List<GetUser>> GetUsers()
	{
		return await SendGetRequest<List<GetUser>>($"api/admin/users/");
	}

	public async Task<HttpResponseMessage> ToggleRole(int id, ToggleRole toggleRole)
	{
		return await SendRequest(new HttpMethod("PATCH"), $"api/admin/users/{id}/toggle-role", JsonContent.Create(toggleRole));
	}

	public async Task<HttpResponseMessage> DeleteUserById(int id)
	{
		return await SendRequest(new HttpMethod("DELETE"), $"api/admin/users/{id}");
	}

}

#pragma warning restore CS0105, CS1591, CS8618, S1128, SA1001, SA1027, SA1028, SA1101, SA1122, SA1137, SA1200, SA1201, SA1208, SA1210, SA1309, SA1311, SA1413, SA1503, SA1505, SA1507, SA1508, SA1516, SA1600, SA1601, SA1602, SA1623, SA1649
