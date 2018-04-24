namespace DevilDaggersWebsite.Models
{
	public class Spawn
	{
		public Enemy Enemy { get; set; }
		public double Delay { get; set; }

		public Spawn(Enemy enemy, double delay)
		{
			Enemy = enemy;
			Delay = delay;
		}
	}
}