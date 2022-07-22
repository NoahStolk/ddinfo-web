using DevilDaggersInfo.Api.Ddre.ProcessMemory;
using DevilDaggersInfo.Razor.ReplayEditor.HttpClients;

namespace DevilDaggersInfo.Razor.ReplayEditor.Services;

public class NetworkService
{
	private readonly DdreApiHttpClient _apiClient;

	public NetworkService()
	{
		_apiClient = new(new() { BaseAddress = new("https://devildaggers.info") });
	}

	public async Task<long> GetMarker(SupportedOperatingSystem operatingSystem)
	{
		GetMarker marker = await _apiClient.GetMarker(operatingSystem);
		return marker.Value;
	}
}
