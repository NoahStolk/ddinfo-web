using Blazored.LocalStorage;
using DevilDaggersInfo.Web.Client.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.Client.HttpClients;

public abstract class ApiHttpClient
{
	private readonly HttpClient _client;
	private readonly ILocalStorageService _localStorageService;

	protected ApiHttpClient(HttpClient client, ILocalStorageService localStorageService)
	{
		_client = client;
		_localStorageService = localStorageService;
	}

	public HttpClient Client => _client;

	protected async Task<HttpResponseMessage> SendRequest(HttpMethod httpMethod, string url, JsonContent? body = null)
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

	protected async Task<T> SendGetRequest<T>(string url)
	{
		HttpResponseMessage response = await SendRequest(HttpMethod.Get, url);
		if (response.StatusCode != HttpStatusCode.OK)
			throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);

		return await response.Content.ReadFromJsonAsync<T>() ?? throw new JsonDeserializationException($"Deserialization error in {url} for JSON '{response.Content}'.");
	}
}
