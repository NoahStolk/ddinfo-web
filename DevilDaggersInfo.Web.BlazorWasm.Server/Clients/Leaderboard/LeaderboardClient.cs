namespace DevilDaggersInfo.Web.BlazorWasm.Server.Clients.Leaderboard;

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

	private async Task<byte[]> ExecuteRequest(string url, params KeyValuePair<string?, string?>[] parameters)
	{
		using FormUrlEncodedContent content = new(parameters);
		using HttpResponseMessage response = await _httpClient.PostAsync(url, content);
		return await response.Content.ReadAsByteArrayAsync();
	}

	public async Task<LeaderboardResponse?> GetLeaderboard(int rankStart)
	{
		try
		{
			byte[] response = await ExecuteRequest(_getScoresUrl, new KeyValuePair<string?, string?>("offset", (rankStart - 1).ToString()));
			return _leaderboardResponseParser.ParseGetLeaderboardResponse(response);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to fetch data from {url} with rank '{rank}'.", _getScoresUrl, rankStart);
			return null;
		}
	}

	public async Task<List<EntryResponse>?> GetEntriesByName(string name)
	{
		if (name.Length < 3 || name.Length > 16)
			throw new ArgumentOutOfRangeException(nameof(name));

		try
		{
			byte[] response = await ExecuteRequest(_getUserSearchUrl, new KeyValuePair<string?, string?>("search", name));
			return _leaderboardResponseParser.ParseGetEntriesByName(response, name);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to fetch data from {url} with search '{search}'.", _getUserSearchUrl, name);
			return null;
		}
	}

	public async Task<List<EntryResponse>?> GetEntriesByIds(IEnumerable<int> ids)
	{
		try
		{
			byte[] response = await ExecuteRequest(_getUsersByIdsUrl, new KeyValuePair<string?, string?>("uid", string.Join(',', ids)));
			return _leaderboardResponseParser.ParseGetEntriesByIds(response, ids.Count());
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to fetch data from {url} with {count} ids.", _getUsersByIdsUrl, ids.Count());
			return null;
		}
	}

	public async Task<EntryResponse?> GetEntryById(int id)
	{
		try
		{
			byte[] response = await ExecuteRequest(_getUserByIdUrl, new KeyValuePair<string?, string?>("uid", id.ToString()));
			return _leaderboardResponseParser.ParseGetEntryById(response);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to fetch data from {url} with id '{id}'.", _getUserByIdUrl, id);
			return null;
		}
	}
}
