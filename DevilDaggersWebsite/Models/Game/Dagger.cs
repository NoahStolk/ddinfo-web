namespace DevilDaggersWebsite.Models.Game
{
	public class Dagger
	{
		public string Name { get; set; }
		public string ColorCode { get; set; }
		public int? UnlockSecond { get; set; }

		public Dagger(string name, string colorCode, int? unlockSecond)
		{
			Name = name;
			ColorCode = colorCode;
			UnlockSecond = unlockSecond;
		}
	}
}