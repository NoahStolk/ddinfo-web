using DevilDaggersWebsite.Dto.Admin;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Entities
{
	public class Title : IAdminUpdatableEntity<AdminTitle>
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; } = null!;
		public List<PlayerTitle> PlayerTitles { get; set; } = new();

		public void Create(ApplicationDbContext dbContext, AdminTitle adminDto)
		{
			Edit(dbContext, adminDto);

			dbContext.Titles.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminTitle adminDto)
		{
			Name = adminDto.Name;

			dbContext.PlayerTitles.RemoveRange(PlayerTitles);
			dbContext.PlayerTitles.AddRange(adminDto.PlayerIds.ConvertAll(pi => new PlayerTitle { PlayerId = pi, TitleId = Id }));
		}

		public AdminTitle Populate()
		{
			return new()
			{
				Name = Name,
				PlayerIds = PlayerTitles.ConvertAll(pt => pt.PlayerId),
			};
		}
	}
}
