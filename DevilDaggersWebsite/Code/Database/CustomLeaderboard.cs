using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Code.Database
{
	public class CustomLeaderboard
	{
		public int Id { get; set; }

		public int CategoryId { get; set; }

		[ForeignKey(nameof(CategoryId))]
		public CustomLeaderboardCategory Category { get; set; }

		public int SpawnsetFileId { get; set; }

		[ForeignKey(nameof(SpawnsetFileId))]
		public SpawnsetFile SpawnsetFile { get; set; }

		public int Bronze { get; set; }

		public int Silver { get; set; }

		public int Golden { get; set; }

		public int Devil { get; set; }

		public int Homing { get; set; }

		public DateTime? DateLastPlayed { get; set; }

		public DateTime? DateCreated { get; set; }

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