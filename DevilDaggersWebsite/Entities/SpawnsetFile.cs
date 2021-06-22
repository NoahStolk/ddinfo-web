using DevilDaggersWebsite.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class SpawnsetFile : IAdminUpdatableEntity<AdminSpawnsetFile>
	{
		[Key]
		public int Id { get; set; }

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; } = null!;

		[StringLength(64)]
		public string Name { get; set; } = null!;

		public int? MaxDisplayWaves { get; set; }

		[StringLength(2048)]
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
			PlayerId = adminDto.PlayerId;
			Name = adminDto.Name;
			MaxDisplayWaves = adminDto.MaxDisplayWaves;
			HtmlDescription = adminDto.HtmlDescription;
			LastUpdated = adminDto.LastUpdated;
			IsPractice = adminDto.IsPractice;
		}

		public AdminSpawnsetFile Populate()
		{
			return new()
			{
				PlayerId = PlayerId,
				Name = Name,
				MaxDisplayWaves = MaxDisplayWaves,
				HtmlDescription = HtmlDescription,
				LastUpdated = LastUpdated,
				IsPractice = IsPractice,
			};
		}
	}
}
