using DevilDaggersInfo.App.Core.ApiClient.ApiClients;

namespace DevilDaggersInfo.Razor.ReplayEditor.HttpClients;

public partial class DdreApiHttpClient : ApiHttpClient
{
	public DdreApiHttpClient(HttpClient client)
		: base(client)
	{
	}
}
