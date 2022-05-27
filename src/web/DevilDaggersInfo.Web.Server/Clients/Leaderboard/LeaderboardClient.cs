#pragma warning disable S4457 // Parameter validation in "async"/"await" methods should be wrapped

namespace DevilDaggersInfo.Web.Server.Clients.Leaderboard;

public class LeaderboardClient
{
#pragma warning disable S1075 // URIs should not be hardcoded
	private const string _getScoresUrl = "http://dd.hasmodai.com/backend15/get_scores.php";
	private const string _getUserSearchUrl = "http://dd.hasmodai.com/backend16/get_user_search_public.php";
	private const string _getUsersByIdsUrl = "http://l.sorath.com/dd/get_multiple_users_by_id_public.php";
	private const string _getUserByIdUrl = "http://dd.hasmodai.com/backend16/get_user_by_id_public.php";
#pragma warning restore S1075 // URIs should not be hardcoded

	private readonly HttpClient _httpClient;
	private readonly LeaderboardResponseParser _leaderboardResponseParser;
	private readonly ILogger<LeaderboardClient> _logger;

	public LeaderboardClient(HttpClient httpClient, LeaderboardResponseParser leaderboardResponseParser, ILogger<LeaderboardClient> logger)
	{
		_httpClient = httpClient;
		_leaderboardResponseParser = leaderboardResponseParser;
		_logger = logger;
	}

	private async Task<ResponseWrapper<TResponse>> ExecuteAndParse<TResponse>(Func<byte[], TResponse> parser, string url, params KeyValuePair<string?, string?>[] parameters)
		where TResponse : class
	{
		try
		{
			using FormUrlEncodedContent content = new(parameters);
			using HttpResponseMessage response = await _httpClient.PostAsync(url, content);
			if (!response.IsSuccessStatusCode)
				return new($"The leaderboard servers returned an unsuccessful response (HTTP {(int)response.StatusCode} {response.StatusCode}).");

			byte[] bytes = await response.Content.ReadAsByteArrayAsync();
			return new(parser(bytes));
		}
		catch (Exception ex)
		{
			LogError(ex, url, parameters);
			return new("The response from the leaderboard servers could not be parsed.");
		}
	}

	public async Task<ResponseWrapper<LeaderboardResponse>> GetLeaderboard(int rankStart)
	{
		return await ExecuteAndParse(
			b => _leaderboardResponseParser.ParseGetLeaderboardResponse(b),
			_getScoresUrl,
			new KeyValuePair<string?, string?>("offset", (rankStart - 1).ToString()));
	}

	public async Task<ResponseWrapper<List<EntryResponse>>> GetEntriesByName(string name)
	{
		if (name.Length < 3 || name.Length > 16)
			throw new ArgumentOutOfRangeException(nameof(name));

		return await ExecuteAndParse(
			b => _leaderboardResponseParser.ParseGetEntriesByName(b),
			_getUserSearchUrl,
			new KeyValuePair<string?, string?>("search", name));
	}

	public async Task<ResponseWrapper<List<EntryResponse>>> GetEntriesByIds(IEnumerable<int> ids)
	{
		return await ExecuteAndParse(
			b => _leaderboardResponseParser.ParseGetEntriesByIds(b),
			_getUsersByIdsUrl,
			new KeyValuePair<string?, string?>("uid", string.Join(',', ids)));
	}

	public async Task<ResponseWrapper<EntryResponse>> GetEntryById(int id)
	{
		return await ExecuteAndParse(
			b => _leaderboardResponseParser.ParseGetEntryById(b),
			_getUserByIdUrl,
			new KeyValuePair<string?, string?>("uid", id.ToString()));
	}

	private void LogError(Exception ex, string url, params KeyValuePair<string?, string?>[] parameters)
	{
		string error = ex switch
		{
			HttpRequestException => "HTTP error",
			EndOfStreamException => "incomplete response",
			_ => "unexpected error",
		};
		_logger.LogError(ex, "Error ({error}) while attempting to fetch data from {url} with parameters: {parameters}", error, url, string.Join(", ", parameters.Select(p => $"{p.Key}: {p.Value}")));
	}
}
