using DevilDaggersInfo.Api.Ddiam;
using DevilDaggersInfo.Razor.AppManager.HttpClients;

namespace DevilDaggersInfo.Razor.AppManager.Services;

public class NetworkService
{
	private readonly DdiamApiHttpClient _apiClient;

	public NetworkService()
	{
		_apiClient = new(new() { BaseAddress = new("https://devildaggers.info") });
	}

	public async Task<List<GetApp>> GetApps(OperatingSystemType operatingSystem)
	{
		return await _apiClient.GetApps(operatingSystem);
	}
}
