namespace DevilDaggersWebsite.Code.Users
{
	public class Ban
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public int? IdResponsible { get; set; }

		public Ban(int id, string description, int? idResponsible)
		{
			Id = id;
			Description = description;
			IdResponsible = idResponsible;
		}
	}
}