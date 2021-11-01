namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public partial class PublicApiHttpClient
{
	private readonly HttpClient _client;

	public PublicApiHttpClient(HttpClient client)
	{
		_client = client;
	}
}
