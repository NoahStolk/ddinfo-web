using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public abstract class ApiHttpClient
{
	protected abstract Task<HttpResponseMessage> SendRequest(HttpMethod httpMethod, string url, JsonContent? body = null);

	protected async Task<T> SendGetRequest<T>(string url)
	{
		HttpResponseMessage response = await SendRequest(HttpMethod.Get, url);
		return await response.Content.ReadFromJsonAsync<T>() ?? throw new JsonDeserializationException();
	}
}
