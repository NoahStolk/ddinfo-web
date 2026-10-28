#pragma warning disable S1075 // URIs should not be hardcoded
#pragma warning disable S4457 // Parameter validation in "async"/"await" methods should be wrapped

using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Clients.Leaderboard;

internal sealed class DdLeaderboardService(HttpClient httpClient, DdLeaderboardCircuitBreaker circuitBreaker, ILogger<DdLeaderboardService> logger) : IDdLeaderboardService
{
	private static readonly Uri _getScoresUrl = new("http://dd.hasmodai.com/dd3/get_scores.php");
	private static readonly Uri _getUserSearchUrl = new("http://dd.hasmodai.com/dd3/get_user_search_public.php");
	private static readonly Uri _getUsersByIdsUrl = new("http://dd.hasmodai.com/dd3/get_multiple_users_by_id_public.php");
	private static readonly Uri _getUserByIdUrl = new("http://dd.hasmodai.com/dd3/get_user_by_id_public.php");

	private async Task<TResponse> ExecuteAndParse<TResponse>(Func<byte[], TResponse> parser, Uri url, params KeyValuePair<string?, string?>[] parameters)
		where TResponse : class
	{
		if (!circuitBreaker.TryAcquire())
			throw new DdLeaderboardException("The leaderboard servers are currently unavailable. Please try again later.");

		byte[] bytes;
		try
		{
			using FormUrlEncodedContent content = new(parameters);
			using HttpResponseMessage response = await httpClient.PostAsync(url, content);
			if (!response.IsSuccessStatusCode)
				throw new DdLeaderboardException($"The leaderboard servers returned an unsuccessful response (HTTP {(int)response.StatusCode} {response.StatusCode}).");

			bytes = await response.Content.ReadAsByteArrayAsync();
		}
		catch (Exception ex)
		{
			DdLeaderboardException exception = ex as DdLeaderboardException ?? new DdLeaderboardException("The leaderboard servers could not be reached.", ex);
			RecordFailure(exception, false, url, parameters);
			throw exception;
		}

		TResponse result;
		try
		{
			result = parser(bytes);
		}
		catch (Exception ex)
		{
			DdLeaderboardException exception = new("The response from the leaderboard servers could not be parsed.", ex);
			RecordFailure(exception, true, url, parameters);
			throw exception;
		}

		if (circuitBreaker.RecordSuccess())
			logger.LogWarning("The leaderboard servers are responding again.");

		return result;
	}

	public async Task<IDdLeaderboardService.LeaderboardResponse> GetLeaderboard(int rankStart, int limit)
	{
		return await ExecuteAndParse(
			r => LeaderboardResponseParser.ParseGetLeaderboardResponse(r, limit),
			_getScoresUrl,
			new KeyValuePair<string?, string?>("offset", (rankStart - 1).ToString()));
	}

	public async Task<List<IDdLeaderboardService.EntryResponse>> GetEntriesByName(string name)
	{
		if (name.Length is < 3 or > 16)
			throw new ArgumentOutOfRangeException(nameof(name));

		return await ExecuteAndParse(
			LeaderboardResponseParser.ParseGetEntriesByName,
			_getUserSearchUrl,
			new KeyValuePair<string?, string?>("search", name));
	}

	public async Task<List<IDdLeaderboardService.EntryResponse>> GetEntriesByIds(IEnumerable<int> ids)
	{
		return await ExecuteAndParse(
			LeaderboardResponseParser.ParseGetEntriesByIds,
			_getUsersByIdsUrl,
			new KeyValuePair<string?, string?>("uid", string.Join(',', ids)));
	}

	public async Task<IDdLeaderboardService.EntryResponse> GetEntryById(int id)
	{
		return await ExecuteAndParse(
			LeaderboardResponseParser.ParseGetEntryById,
			_getUserByIdUrl,
			new KeyValuePair<string?, string?>("uid", id.ToString()));
	}

	private void RecordFailure(DdLeaderboardException exception, bool isParseError, Uri url, KeyValuePair<string?, string?>[] parameters)
	{
		string parametersString = string.Join(", ", parameters.Select(p => $"{p.Key}: {p.Value}"));
		switch (circuitBreaker.RecordFailure())
		{
			case DdLeaderboardCircuitBreaker.FailureOutcome.Opened:
				logger.LogError(
					exception,
					"The leaderboard servers failed {Count} times in a row. Requests will fail immediately, and the servers will be retried every {BreakDuration}. Last failure while fetching data from {Url} with parameters: {Parameters}",
					DdLeaderboardCircuitBreaker.FailureThreshold,
					DdLeaderboardCircuitBreaker.BreakDuration,
					url,
					parametersString);
				break;

			// Parse errors outside an outage most likely mean the parser does not handle a response correctly, so these are always reported.
			case DdLeaderboardCircuitBreaker.FailureOutcome.Closed when isParseError:
				logger.LogError(exception, "Error while parsing data from {Url} with parameters: {Parameters}", url, parametersString);
				break;

			// Transient failures, or failures during an outage that has already been reported.
			default:
				logger.LogInformation(exception, "Error while fetching data from {Url} with parameters: {Parameters}", url, parametersString);
				break;
		}
	}
}
