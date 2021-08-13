using System.Net.Http;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients
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
