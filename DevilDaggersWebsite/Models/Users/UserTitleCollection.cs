namespace DevilDaggersWebsite.Models.Users
{
	public class UserTitleCollection
	{
		public int ID { get; set; }
		public string[] Titles { get; set; }

		public UserTitleCollection(int id, string[] titles)
		{
			ID = id;
			Titles = titles;
		}
	}
}