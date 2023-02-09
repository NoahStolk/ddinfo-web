namespace DevilDaggersInfo.App.Launcher;

public static class Client
{
	private static readonly Uri _baseAddress = new("https://devildaggers.info/");

	public static async Task<bool> IsLatestVersionAsync(string toolName, string version)
	{
		using HttpRequestMessage request = new()
		{
			RequestUri = new($"api/app-launcher/is-latest-version?toolName={toolName}&version={version}&publishMethod={Constants.PublishMethod}&buildType={Constants.BuildType}", UriKind.Relative),
			Method = HttpMethod.Head,
		};

		using HttpClient httpClient = new() { BaseAddress = _baseAddress };

		using HttpResponseMessage response = await httpClient.SendAsync(request);
		if (response.IsSuccessStatusCode)
			return false;

		string error = await response.Content.ReadAsStringAsync();
		Cmd.WriteLine(ConsoleColor.Red, error);
		return true;
	}

	public static async Task<byte[]> DownloadAppAsync()
	{
		using HttpRequestMessage request = new()
		{
			RequestUri = new($"api/app-launcher/latest-version-file?publishMethod={Constants.PublishMethod}&buildType={Constants.BuildType}", UriKind.Relative),
			Method = HttpMethod.Get,
		};

		using HttpClient httpClient = new() { BaseAddress = _baseAddress };

		using HttpResponseMessage response = await httpClient.SendAsync(request);
		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);

		return await response.Content.ReadAsByteArrayAsync();
	}
}
