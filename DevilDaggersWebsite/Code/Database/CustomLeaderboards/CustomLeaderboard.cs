namespace DevilDaggersWebsite.Code.Database.CustomLeaderboards
{
	public class CustomLeaderboard
	{
		public int ID { get; set; }
		public string SpawnsetFileName { get; set; }
		public float Bronze { get; set; }
		public float Silver { get; set; }
		public float Golden { get; set; }
		public float Devil { get; set; }
		public float Homing { get; set; }

		/// <summary>
		/// Returns the CSS class name corresponding to the time in seconds.
		/// </summary>
		/// <param name="time">The time in seconds.</param>
		/// <returns>The CSS class name for the dagger.</returns>
		public string GetDagger(float time)
		{
			if (time >= Homing && Homing > 0)
				return "homing";
			if (time >= Devil && Devil > 0)
				return "devil";
			if (time >= Golden && Golden > 0)
				return "golden";
			if (time >= Silver && Silver > 0)
				return "silver";
			if (time >= Bronze && Bronze > 0)
				return "bronze";
			return "default";
		}
	}
}