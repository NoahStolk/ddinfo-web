namespace DevilDaggersInfo.Web.Server.Clients.Clubber;

public class ClubberClient
{
	private readonly HttpClient _httpClient;
	private readonly ILogger<ClubberClient> _logger;

	public ClubberClient(HttpClient httpClient, ILogger<ClubberClient> logger)
	{
		_httpClient = httpClient;
		_logger = logger;
	}

	public async Task<List<DdUser>?> GetUsers()
	{
		try
		{
			return await _httpClient.GetFromJsonAsync<List<DdUser>>("https://clubber.onrender.com/users") ?? throw new InvalidOperationException("Response was null which was not expected.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to request users from Clubber.");
			return null;
		}
	}
}
