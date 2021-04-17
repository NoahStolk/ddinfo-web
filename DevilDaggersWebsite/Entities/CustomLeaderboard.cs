using DevilDaggersWebsite.Dto.Admin;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class CustomLeaderboard : IAdminUpdatableEntity<AdminCustomLeaderboard>
	{
		[Key]
		public int Id { get; set; }

		public CustomLeaderboardCategory Category { get; set; }

		public int SpawnsetFileId { get; set; }

		[ForeignKey(nameof(SpawnsetFileId))]
		public SpawnsetFile SpawnsetFile { get; set; } = null!;

		public int TimeBronze { get; set; }

		public int TimeSilver { get; set; }

		public int TimeGolden { get; set; }

		public int TimeDevil { get; set; }

		public int TimeLeviathan { get; set; }

		public DateTime? DateLastPlayed { get; set; }

		public DateTime? DateCreated { get; set; }

		public int TotalRunsSubmitted { get; set; }

		public bool IsArchived { get; set; }

		/// <summary>
		/// Returns the CSS class name corresponding to the time in seconds.
		/// </summary>
		/// <param name="time">The time in tenths of milliseconds.</param>
		/// <returns>The CSS class name for the dagger.</returns>
		public string GetDagger(int time)
		{
			if (Category.IsAscending())
			{
				if (time <= TimeLeviathan)
					return "leviathan";
				if (time <= TimeDevil)
					return "devil";
				if (time <= TimeGolden)
					return "golden";
				if (time <= TimeSilver)
					return "silver";
				if (time <= TimeBronze)
					return "bronze";
				return "default";
			}

			if (time >= TimeLeviathan)
				return "leviathan";
			if (time >= TimeDevil)
				return "devil";
			if (time >= TimeGolden)
				return "golden";
			if (time >= TimeSilver)
				return "silver";
			if (time >= TimeBronze)
				return "bronze";
			return "default";
		}

		public void Create(ApplicationDbContext dbContext, AdminCustomLeaderboard adminDto)
		{
			DateCreated = DateTime.UtcNow;

			Edit(dbContext, adminDto);

			dbContext.CustomLeaderboards.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminCustomLeaderboard adminDto)
		{
			Category = adminDto.Category;
			SpawnsetFileId = adminDto.SpawnsetFileId;
			TimeBronze = adminDto.TimeBronze;
			TimeSilver = adminDto.TimeSilver;
			TimeGolden = adminDto.TimeGolden;
			TimeDevil = adminDto.TimeDevil;
			TimeLeviathan = adminDto.TimeLeviathan;
			IsArchived = adminDto.IsArchived;
		}

		public AdminCustomLeaderboard Populate()
		{
			return new()
			{
				Category = Category,
				SpawnsetFileId = SpawnsetFileId,
				TimeBronze = TimeBronze,
				TimeDevil = TimeDevil,
				TimeGolden = TimeGolden,
				TimeLeviathan = TimeLeviathan,
				TimeSilver = TimeSilver,
				IsArchived = IsArchived,
			};
		}
	}
}
