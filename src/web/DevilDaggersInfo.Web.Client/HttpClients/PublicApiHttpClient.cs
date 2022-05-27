using Blazored.LocalStorage;
using DevilDaggersInfo.Web.Client.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.Client.HttpClients;

public partial class PublicApiHttpClient : ApiHttpClient
{
	public PublicApiHttpClient(HttpClient client, ILocalStorageService localStorageService)
		: base(client, localStorageService)
	{
	}
}
