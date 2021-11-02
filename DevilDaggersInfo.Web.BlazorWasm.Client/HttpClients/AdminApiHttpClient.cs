using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public partial class AdminApiHttpClient
{
	private readonly HttpClient _client;
	private readonly ILocalStorageService _localStorageService;

	public AdminApiHttpClient(HttpClient client, ILocalStorageService localStorageService)
	{
		_client = client;
		_localStorageService = localStorageService;
	}

	private async Task<HttpResponseMessage> SendRequest(HttpMethod httpMethod, string url, JsonContent? body = null)
	{
		HttpRequestMessage request = new()
		{
			RequestUri = new Uri(url, UriKind.Relative),
			Method = httpMethod,
			Content = body,
		};
		string? token = await _localStorageService.GetItemAsStringAsync("auth"); // TODO: Don't hardcode.
		if (token != null)
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

		return await _client.SendAsync(request);
	}

	private async Task<T> SendGetRequest<T>(string url)
	{
		HttpResponseMessage response = await SendRequest(HttpMethod.Get, url);
		return await response.Content.ReadFromJsonAsync<T>() ?? throw new JsonDeserializationException();
	}
}
