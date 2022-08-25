using DevilDaggersInfo.App.Core.ApiClient;

namespace DevilDaggersInfo.Razor.SurvivalEditor.HttpClients;

public partial class DdseApiHttpClient : ApiHttpClient
{
	public DdseApiHttpClient(HttpClient client)
		: base(client)
	{
	}
}
