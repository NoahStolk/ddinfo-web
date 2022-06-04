using System.Net;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Core.CustomLeaderboards.HttpClients;

public partial class DdclApiHttpClient
{
	private readonly HttpClient _client;

	protected DdclApiHttpClient(HttpClient client)
	{
		_client = client;
	}

	protected async Task<HttpResponseMessage> SendRequest(HttpMethod httpMethod, string url, JsonContent? body = null)
	{
		HttpRequestMessage request = new()
		{
			RequestUri = new Uri(url, UriKind.Relative),
			Method = httpMethod,
			Content = body,
		};

		return await _client.SendAsync(request);
	}

	protected async Task<T> SendGetRequest<T>(string url)
	{
		HttpResponseMessage response = await SendRequest(HttpMethod.Get, url);
		if (response.StatusCode != HttpStatusCode.OK)
			throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);

		return await response.Content.ReadFromJsonAsync<T>() ?? throw new($"Deserialization error in {url} for JSON '{response.Content}'.");
	}
}
