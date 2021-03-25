using DevilDaggersWebsite.Dto.Admin;
using DevilDaggersWebsite.Enumerators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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

		public bool IsAscending()
			=> Category == CustomLeaderboardCategory.Challenge || Category == CustomLeaderboardCategory.Speedrun;

		public void Create(ApplicationDbContext dbContext, AdminCustomLeaderboard adminDto, StringBuilder auditLogger)
		{
			DateCreated = DateTime.UtcNow;

			Edit(dbContext, adminDto, auditLogger);

			dbContext.CustomLeaderboards.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminCustomLeaderboard adminDto, StringBuilder auditLogger)
		{
			(this as IAdminUpdatableEntity<AdminCustomLeaderboard>).TrackEditUpdates(auditLogger, adminDto, typeof(CustomLeaderboard));

			Category = adminDto.Category;
			SpawnsetFileId = adminDto.SpawnsetFileId;
			TimeBronze = adminDto.TimeBronze;
			TimeSilver = adminDto.TimeSilver;
			TimeGolden = adminDto.TimeGolden;
			TimeDevil = adminDto.TimeDevil;
			TimeLeviathan = adminDto.TimeLeviathan;
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
			};
		}
	}
}
