using Blazored.LocalStorage;

namespace DevilDaggersInfo.Web.Client.HttpClients;

public partial class MainApiHttpClient : ApiHttpClient
{
	public MainApiHttpClient(HttpClient client, ILocalStorageService localStorageService)
		: base(client, localStorageService)
	{
	}
}
