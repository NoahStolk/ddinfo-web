using DevilDaggersWebsite.BlazorWasm.Client.Utils;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.LeaderboardHistoryStatistics;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.Leaderboards;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.Players;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums.Sortings.Public;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Client.HttpClients
{
	public class PublicApiHttpClient
	{
		private readonly HttpClient _client;

		public PublicApiHttpClient(HttpClient client)
		{
			_client = client;
		}

		public async Task<GetCustomLeaderboard> GetCustomLeaderboardById(int id)
		{
			return await _client.GetFromJsonAsync<GetCustomLeaderboard>($"api/custom-leaderboards/{id}") ?? throw new JsonSerializationException();
		}

		public async Task<Page<GetCustomLeaderboardOverview>> GetCustomLeaderboards(CustomLeaderboardCategory category, int pageIndex, int pageSize, CustomLeaderboardSorting sortBy, bool ascending)
		{
			Dictionary<string, object?> queryParameters = new()
			{
				{ nameof(category), category },
				{ nameof(pageIndex), pageIndex },
				{ nameof(pageSize), pageSize },
				{ nameof(sortBy), sortBy },
				{ nameof(ascending), ascending },
			};
			return await _client.GetFromJsonAsync<Page<GetCustomLeaderboardOverview>>(UrlBuilderUtils.BuildUrlWithQuery("api/custom-leaderboards", queryParameters)) ?? throw new JsonSerializationException();
		}

		public async Task<List<GetLeaderboardHistoryStatistics>> GetLeaderboardHistoryStatistics()
		{
			return await _client.GetFromJsonAsync<List<GetLeaderboardHistoryStatistics>>("api/leaderboard-history-statistics") ?? throw new JsonSerializationException();
		}

		public async Task<List<GetPlayerForLeaderboard>> GetPlayersForLeaderboard()
		{
			return await _client.GetFromJsonAsync<List<GetPlayerForLeaderboard>>("api/players/leaderboard") ?? throw new JsonSerializationException();
		}

		public async Task<GetLeaderboardStatistics> GetLeaderboardStatistics()
		{
			return await _client.GetFromJsonAsync<GetLeaderboardStatistics>("api/leaderboard-statistics") ?? throw new JsonSerializationException();
		}

		public async Task<GetLeaderboard> GetLeaderboard(int rankStart)
		{
			return await _client.GetFromJsonAsync<GetLeaderboard>(UrlBuilderUtils.BuildUrlWithQuery("api/leaderboards", new() { { nameof(rankStart), rankStart } })) ?? throw new JsonSerializationException();
		}
	}
}
