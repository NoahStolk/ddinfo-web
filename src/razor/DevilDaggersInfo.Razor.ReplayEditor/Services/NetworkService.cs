using DevilDaggersInfo.Api.Ddre.ProcessMemory;
using DevilDaggersInfo.Razor.ReplayEditor.HttpClients;

namespace DevilDaggersInfo.Razor.ReplayEditor.Services;

public class NetworkService
{
	private readonly DdreApiHttpClient _apiClient;

	private long? _marker;

	public NetworkService()
	{
		_apiClient = new(new() { BaseAddress = new("https://devildaggers.info") });
	}

	public async Task<long> GetMarkerAsync(SupportedOperatingSystem operatingSystem)
	{
		if (_marker.HasValue)
			return _marker.Value;

		GetMarker marker = await _apiClient.GetMarker(operatingSystem);
		_marker = marker.Value;
		return _marker.Value;
	}
}
