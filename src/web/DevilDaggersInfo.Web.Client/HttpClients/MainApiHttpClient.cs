using Blazored.LocalStorage;
using DevilDaggersInfo.Web.Client.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.Client.HttpClients;

public partial class MainApiHttpClient : ApiHttpClient
{
	public MainApiHttpClient(HttpClient client, ILocalStorageService localStorageService)
		: base(client, localStorageService)
	{
	}
}
