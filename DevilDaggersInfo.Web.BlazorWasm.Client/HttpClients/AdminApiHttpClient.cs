using System.Net.Http;

namespace DevilDaggersWebsite.BlazorWasm.Client.HttpClients
{
	public class AdminApiHttpClient
	{
		public AdminApiHttpClient(HttpClient client)
		{
			Client = client;
		}

		public HttpClient Client { get; }
	}
}
