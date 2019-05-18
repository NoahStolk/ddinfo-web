namespace DevilDaggersWebsite.Code.Users
{
	public class Ban
	{
		public int ID { get; set; }
		public string Description { get; set; }
		public int? IDResponsible { get; set; }

		public Ban(int id, string description, int? idResponsible)
		{
			ID = id;
			Description = description;
			IDResponsible = idResponsible;
		}
	}
}