namespace DevilDaggersWebsite.Code.Users
{
	public class UserTitleCollection : AbstractUserData
	{
		public override string FileName => "titles";

		public int Id { get; set; }
		public string[] Titles { get; set; }

		public UserTitleCollection()
		{
		}

		public UserTitleCollection(int id, string[] titles)
		{
			Id = id;
			Titles = titles;
		}
	}
}