using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public partial class PublicApiHttpClient : ApiHttpClient
{
	private readonly HttpClient _client;

	public PublicApiHttpClient(HttpClient client)
	{
		_client = client;
	}

	protected override async Task<HttpResponseMessage> SendRequest(HttpMethod httpMethod, string url, JsonContent? body = null)
	{
		HttpRequestMessage request = new()
		{
			RequestUri = new Uri(url, UriKind.Relative),
			Method = httpMethod,
			Content = body,
		};

		return await _client.SendAsync(request);
	}
}
