using DevilDaggersCore.CustomLeaderboards;

namespace DevilDaggersWebsite.Code.Database.CustomLeaderboards
{
	public class CustomLeaderboard : CustomLeaderboardBase
	{
		public int ID { get; set; }

		public CustomLeaderboard(string spawnsetFileName, float bronze, float silver, float golden, float devil, float homing)
			: base(spawnsetFileName, bronze, silver, golden, devil, homing)
		{
		}

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