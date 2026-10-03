using Blazored.LocalStorage;
using DevilDaggersInfo.Web.Client.Authentication;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Client.HttpClients;

public abstract class ApiHttpClient
{
	private readonly ILocalStorageService _localStorageService;

	protected ApiHttpClient(HttpClient client, ILocalStorageService localStorageService)
	{
		Client = client;
		_localStorageService = localStorageService;
	}

	public HttpClient Client { get; }

	protected async Task<HttpResponseMessage> SendRequest(HttpMethod httpMethod, string url, JsonContent? body = null)
	{
		using HttpRequestMessage request = new();
		request.RequestUri = new Uri(url, UriKind.Relative);
		request.Method = httpMethod;
		request.Content = body;
		string? token = await _localStorageService.GetItemAsStringAsync(AdminAuthenticationStateProvider.LocalStorageAuthKey);
		if (token != null)
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

		return await Client.SendAsync(request);
	}

	protected async Task<T> SendGetRequest<T>(string url)
	{
		HttpResponseMessage response = await SendRequest(HttpMethod.Get, url);
		if (response.StatusCode != HttpStatusCode.OK)
			throw new HttpRequestException(await ReadErrorMessage(response), null, response.StatusCode);

		return await response.Content.ReadFromJsonAsync<T>() ?? throw new InvalidDataException($"Deserialization error in {url} for JSON '{response.Content}'.");
	}

	private static async Task<string> ReadErrorMessage(HttpResponseMessage response)
	{
		string content = await response.Content.ReadAsStringAsync();
		if (response.Content.Headers.ContentType?.MediaType != "application/problem+json")
			return content;

		// Errors thrown by the API are problem details with a displayable title. Validation errors are kept whole, because their title alone doesn't say what is wrong.
		try
		{
			using JsonDocument document = JsonDocument.Parse(content);
			JsonElement root = document.RootElement;
			if (root.ValueKind == JsonValueKind.Object && !root.TryGetProperty("errors", out _) && root.TryGetProperty("title", out JsonElement title) && title.ValueKind == JsonValueKind.String && title.GetString() is { } titleString)
				return titleString;
		}
		catch (JsonException)
		{
			// Fall back to the raw content.
		}

		return content;
	}

	protected static string BuildUrlWithQuery(string baseUrl, Dictionary<string, object?> queryParameters)
	{
		if (queryParameters.Count == 0)
			return baseUrl;

		string queryParameterString = string.Join('&', queryParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
		return $"{baseUrl.TrimEnd('/')}?{queryParameterString}";
	}
}
