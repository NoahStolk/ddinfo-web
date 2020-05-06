using DevilDaggersCore.CustomLeaderboards;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Code.Database.CustomLeaderboards
{
	public class CustomLeaderboard : CustomLeaderboardBase
	{
		public int Id { get; set; }

		public int CategoryId { get; set; }

		[ForeignKey("CategoryId")]
		public CustomLeaderboardCategory Category { get; set; }

		public CustomLeaderboard(string spawnsetFileName, int bronze, int silver, int golden, int devil, int homing, DateTime? dateLastPlayed, DateTime? dateCreated)
			: base(spawnsetFileName, bronze, silver, golden, devil, homing, dateLastPlayed, dateCreated)
		{
		}

		/// <summary>
		/// Returns the CSS class name corresponding to the time in seconds.
		/// </summary>
		/// <param name="time">The time in seconds.</param>
		/// <returns>The CSS class name for the dagger.</returns>
		public string GetDagger(float time)
		{
			if (Category.Ascending)
			{
				if (time <= Homing && Homing > 0)
					return "homing";
				if (time <= Devil && Devil > 0)
					return "devil";
				if (time <= Golden && Golden > 0)
					return "golden";
				if (time <= Silver && Silver > 0)
					return "silver";
				if (time <= Bronze && Bronze > 0)
					return "bronze";
				return "default";
			}

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