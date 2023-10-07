using Blazored.LocalStorage;

namespace DevilDaggersInfo.Web.Client.HttpClients;

public partial class AdminApiHttpClient : ApiHttpClient
{
	public AdminApiHttpClient(HttpClient client, ILocalStorageService localStorageService)
		: base(client, localStorageService)
	{
	}
}
