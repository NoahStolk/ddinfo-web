using DevilDaggersWebsite.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevilDaggersWebsite.Entities
{
	public class Title : IAdminUpdatableEntity<AdminTitle>
	{
		[Key]
		public int Id { get; init; }

		[StringLength(16)]
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
		}

		public void CreateManyToManyRelations(ApplicationDbContext dbContext, AdminTitle adminDto)
		{
			List<int> playerIds = adminDto.PlayerIds ?? new();
			foreach (PlayerTitle newEntity in playerIds.ConvertAll(pi => new PlayerTitle { TitleId = Id, PlayerId = pi }))
			{
				if (!dbContext.PlayerTitles.Any(pam => pam.TitleId == newEntity.TitleId && pam.PlayerId == newEntity.PlayerId))
					dbContext.PlayerTitles.Add(newEntity);
			}

			foreach (PlayerTitle entityToRemove in dbContext.PlayerTitles.Where(pam => pam.TitleId == Id && !playerIds.Contains(pam.PlayerId)))
				dbContext.PlayerTitles.Remove(entityToRemove);
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
