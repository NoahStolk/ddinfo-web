using Blazored.LocalStorage;
using DevilDaggersInfo.Web.BlazorWasm.Client.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public partial class AdminApiHttpClient : ApiHttpClient
{
	private readonly HttpClient _client;
	private readonly ILocalStorageService _localStorageService;

	public AdminApiHttpClient(HttpClient client, ILocalStorageService localStorageService)
	{
		_client = client;
		_localStorageService = localStorageService;
	}

	protected override async Task<HttpResponseMessage> SendRequest(HttpMethod httpMethod, string url, JsonContent? body = null)
	{
		HttpRequestMessage request = new()
		{
			RequestUri = new Uri(url, UriKind.Relative),
			Method = httpMethod,
			Content = body,
		};
		string? token = await _localStorageService.GetItemAsStringAsync(AdminAuthenticationStateProvider.LocalStorageAuthKey);
		if (token != null)
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

		return await _client.SendAsync(request);
	}
}
