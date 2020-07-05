namespace DevilDaggersWebsite.Code.Users
{
	public class Donator : AbstractUserData
	{
		public override string FileName => "donators";

		public int Id { get; set; }
		public string UsernameFallback { get; set; }
		public bool IsAnonymous { get; set; }

		public Donator()
		{
		}

		public Donator(int id, string usernameFallback, bool isAnonymous = false)
		{
			Id = id;
			UsernameFallback = usernameFallback;
			IsAnonymous = isAnonymous;
		}
	}
}