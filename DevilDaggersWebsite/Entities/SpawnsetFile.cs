using DevilDaggersWebsite.Dto.Admin;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class SpawnsetFile : IAdminUpdatableEntity<AdminSpawnsetFile>
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; } = null!;

		public int? MaxDisplayWaves { get; set; }

		public string? HtmlDescription { get; set; }

		public DateTime LastUpdated { get; set; }

		public bool IsPractice { get; set; }

		public void Create(ApplicationDbContext dbContext, AdminSpawnsetFile adminDto)
		{
			Edit(dbContext, adminDto);

			dbContext.SpawnsetFiles.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminSpawnsetFile adminDto)
		{
			Name = adminDto.Name;
			HtmlDescription = adminDto.HtmlDescription;
			IsPractice = adminDto.IsPractice;
			MaxDisplayWaves = adminDto.MaxDisplayWaves;
			PlayerId = adminDto.PlayerId;
			LastUpdated = adminDto.LastUpdated;
		}

		public AdminSpawnsetFile Populate()
		{
			return new()
			{
				Name = Name,
				HtmlDescription = HtmlDescription,
				IsPractice = IsPractice,
				MaxDisplayWaves = MaxDisplayWaves,
				PlayerId = PlayerId,
				LastUpdated = LastUpdated,
			};
		}
	}
}
