using System.Net.Http;

namespace DevilDaggersWebsite.Clients
{
	public class DevilDaggersInfoClient
	{
		public DevilDaggersInfoClient(HttpClient client)
		{
			Client = client;
		}

		public HttpClient Client { get; }
	}
}
