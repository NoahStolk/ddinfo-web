namespace DevilDaggersWebsite.Code.Users
{
	public class UserTitleCollection : AbstractUserData
	{
		public override string FileName => "titles";

		public int Id { get; set; }
		public string[] Titles { get; set; }
	}
}