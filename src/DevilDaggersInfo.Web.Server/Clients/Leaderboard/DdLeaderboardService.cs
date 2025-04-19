#pragma warning disable S1075 // URIs should not be hardcoded
#pragma warning disable S4457 // Parameter validation in "async"/"await" methods should be wrapped

using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Clients.Leaderboard;

public class DdLeaderboardService : IDdLeaderboardService
{
	private static readonly Uri _getScoresUrl = new("http://dd.hasmodai.com/dd3/get_scores.php");
	private static readonly Uri _getUserSearchUrl = new("http://dd.hasmodai.com/dd3/get_user_search_public.php");
	private static readonly Uri _getUsersByIdsUrl = new("http://dd.hasmodai.com/dd3/get_multiple_users_by_id_public.php");
	private static readonly Uri _getUserByIdUrl = new("http://dd.hasmodai.com/dd3/get_user_by_id_public.php");
	private static readonly Uri _getRunsUrl = new("http://dd.hasmodai.com/dd3/get_recent.php");

	private readonly HttpClient _httpClient;
	private readonly LeaderboardResponseParser _leaderboardResponseParser;
	private readonly ILogger<DdLeaderboardService> _logger;

	public DdLeaderboardService(HttpClient httpClient, LeaderboardResponseParser leaderboardResponseParser, ILogger<DdLeaderboardService> logger)
	{
		_httpClient = httpClient;
		_leaderboardResponseParser = leaderboardResponseParser;
		_logger = logger;
	}

	private async Task<TResponse> ExecuteAndParse<TResponse>(Func<byte[], TResponse> parser, Uri url, params KeyValuePair<string?, string?>[] parameters)
		where TResponse : class
	{
		try
		{
			using FormUrlEncodedContent content = new(parameters);
			using HttpResponseMessage response = await _httpClient.PostAsync(url, content);
			if (!response.IsSuccessStatusCode)
				throw new DdLeaderboardException($"The leaderboard servers returned an unsuccessful response (HTTP {(int)response.StatusCode} {response.StatusCode}).");

			byte[] bytes = await response.Content.ReadAsByteArrayAsync();
			return parser(bytes);
		}
		catch (Exception ex)
		{
			LogError(ex, url, parameters);
			throw new DdLeaderboardException("The response from the leaderboard servers could not be parsed.");
		}
	}

	public async Task<IDdLeaderboardService.LeaderboardResponse> GetLeaderboard(int rankStart, int limit)
	{
		return await ExecuteAndParse(
			r => _leaderboardResponseParser.ParseGetLeaderboardResponse(r, limit),
			_getScoresUrl,
			new KeyValuePair<string?, string?>("offset", (rankStart - 1).ToString()));
	}

	public async Task<List<IDdLeaderboardService.EntryResponse>> GetEntriesByName(string name)
	{
		if (name.Length is < 3 or > 16)
			throw new ArgumentOutOfRangeException(nameof(name));

		return await ExecuteAndParse(
			_leaderboardResponseParser.ParseGetEntriesByName,
			_getUserSearchUrl,
			new KeyValuePair<string?, string?>("search", name));
	}

	public async Task<List<IDdLeaderboardService.EntryResponse>> GetEntriesByIds(IEnumerable<int> ids)
	{
		return await ExecuteAndParse(
			_leaderboardResponseParser.ParseGetEntriesByIds,
			_getUsersByIdsUrl,
			new KeyValuePair<string?, string?>("uid", string.Join(',', ids)));
	}

	public async Task<IDdLeaderboardService.EntryResponse> GetEntryById(int id)
	{
		return await ExecuteAndParse(
			_leaderboardResponseParser.ParseGetEntryById,
			_getUserByIdUrl,
			new KeyValuePair<string?, string?>("uid", id.ToString()));
	}

	public async Task<List<IDdLeaderboardService.RunResponse>> GetRunsByTimestamp(DateTimeOffset before, int limit)
	{
		long timestamp = before.ToUnixTimeSeconds();
		List<IDdLeaderboardService.RunResponse>? runs = await _httpClient.GetFromJsonAsync<List<IDdLeaderboardService.RunResponse>>($"{_getRunsUrl}?{nameof(before)}={timestamp}?{nameof(limit)}={limit}");
		if (runs != null)
			return runs;

		LogError(null, _getRunsUrl, new(nameof(before), timestamp.ToString()), new(nameof(limit), limit.ToString()));
		throw new DdLeaderboardException("The response from the leaderboard servers could not be parsed.");
	}

	private void LogError(Exception ex, Uri url, params KeyValuePair<string?, string?>[] parameters)
	{
		string error = ex switch
		{
			HttpRequestException => "HTTP error",
			EndOfStreamException => "incomplete response",
			_ => "unexpected error",
		};
		_logger.LogError(ex, "Error ({Error}) while attempting to fetch data from {Url} with parameters: {Parameters}", error, url, string.Join(", ", parameters.Select(p => $"{p.Key}: {p.Value}")));
	}
}
