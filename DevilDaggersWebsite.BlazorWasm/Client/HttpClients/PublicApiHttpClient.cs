using System.Net.Http;

namespace DevilDaggersWebsite.BlazorWasm.Client.HttpClients
{
	public class PublicApiHttpClient
	{
		public PublicApiHttpClient(HttpClient client)
		{
			Client = client;
		}

		public HttpClient Client { get; }
	}
}
