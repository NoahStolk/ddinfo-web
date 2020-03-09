namespace DevilDaggersWebsite.Code.Users
{
	public class UserTitleCollection
	{
		public int Id { get; set; }
		public string[] Titles { get; set; }

		public UserTitleCollection(int id, string[] titles)
		{
			Id = id;
			Titles = titles;
		}
	}
}