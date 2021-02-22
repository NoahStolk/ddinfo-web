using DiscordBotDdInfo.Clients;

namespace DiscordBotDdInfo
{
	public static class NetworkUtils
	{
#if DEBUG
		public static readonly string BaseUrl = "http://localhost:2963";
#else
		public static readonly string BaseUrl = "https://devildaggers.info";
#endif

		static NetworkUtils()
			=> ApiClient = new(new() { BaseAddress = new(BaseUrl), });

		public static DevilDaggersInfoApiClient ApiClient { get; }
	}
}
