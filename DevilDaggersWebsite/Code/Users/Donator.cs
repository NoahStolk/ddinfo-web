namespace DevilDaggersWebsite.Code.Users
{
	public class Donator
	{
		public int Id { get; set; }
		public string UsernameFallback { get; set; }
		public bool IsAnonymous { get; set; }

		public Donator(int id, string usernameFallback, bool isAnonymous = false)
		{
			Id = id;
			UsernameFallback = usernameFallback;
			IsAnonymous = isAnonymous;
		}
	}
}