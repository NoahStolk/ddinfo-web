using DevilDaggersInfo.Razor.SurvivalEditor.HttpClients;

namespace DevilDaggersInfo.Razor.SurvivalEditor.Services;

public class NetworkService
{
	private readonly DdseApiHttpClient _apiClient;

	public NetworkService()
	{
		_apiClient = new(new() { BaseAddress = new("https://devildaggers.info") });
	}
}
