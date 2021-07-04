using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Clients
{
	public class DevilDaggersInfoClient
	{
		public DevilDaggersInfoClient(HttpClient client)
		{
			Client = client;
		}

		public HttpClient Client { get; }

		public async Task<TResult> GetAsync<TResult>(string url, CancellationToken cancellationToken = default)
		{
			HttpResponseMessage responseMessage = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
			string jsonResponse = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
			return JsonConvert.DeserializeObject<TResult>(jsonResponse) ?? throw new($"Could not deserialize response from '{url}' as '{typeof(TResult).Name}'.");
		}
	}
}
