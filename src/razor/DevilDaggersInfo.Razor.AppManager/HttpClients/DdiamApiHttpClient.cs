using DevilDaggersInfo.App.Core.ApiClient;

namespace DevilDaggersInfo.Razor.AppManager.HttpClients;

public partial class DdiamApiHttpClient : ApiHttpClient
{
	public DdiamApiHttpClient(HttpClient client)
		: base(client)
	{
	}
}
