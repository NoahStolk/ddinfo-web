namespace DevilDaggersWebsite.Core.Entities
{
	public class PlayerTitle
	{
		public int PlayerId { get; set; }
		public Player Player { get; set; }

		public int TitleId { get; set; }
		public Title Title { get; set; }
	}
}