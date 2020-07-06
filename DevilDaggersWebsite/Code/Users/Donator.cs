namespace DevilDaggersWebsite.Code.Users
{
	public class Donator : AbstractUserData
	{
		public override string FileName => "donators";

		public int Id { get; set; }
		public string UsernameFallback { get; set; }
		public bool IsAnonymous { get; set; }
	}
}