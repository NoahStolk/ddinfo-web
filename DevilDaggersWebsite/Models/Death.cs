namespace DevilDaggersWebsite.Models
{
	public class Death
	{
		public string Name { get; set; }
		public string ColorCode { get; set; }
		public int Type { get; set; }

		public Death(string name, string colorCode, int type)
		{
			Name = name;
			ColorCode = colorCode;
			Type = type;
		}
	}
}