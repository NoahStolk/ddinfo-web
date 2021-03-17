using DevilDaggersWebsite.Dto.Admin;
using DevilDaggersWebsite.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DevilDaggersWebsite.Entities
{
	public class AssetMod : IAdminUpdatableEntity<AdminAssetMod>
	{
		[Key]
		public int Id { get; set; }

		public List<PlayerAssetMod> PlayerAssetMods { get; set; } = new();
		public AssetModTypes AssetModTypes { get; set; }
		public AssetModFileContents AssetModFileContents { get; set; }
		public string Name { get; set; } = null!;
		public string Url { get; set; } = null!;

		public void Create(ApplicationDbContext dbContext, AdminAssetMod adminDto, StringBuilder auditLogger)
		{
			Edit(dbContext, adminDto, auditLogger);

			dbContext.AssetMods.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminAssetMod adminDto, StringBuilder auditLogger)
		{
			(this as IAdminUpdatableEntity<AdminAssetMod>).TrackEditUpdates(auditLogger, adminDto, typeof(AssetMod));

			AssetModTypes = (AssetModTypes)adminDto.AssetModTypes.Cast<int>().Sum();
			AssetModFileContents = (AssetModFileContents)adminDto.AssetModFileContents.Cast<int>().Sum();
			Name = adminDto.Name;
			Url = adminDto.Url;
		}

		public void CreateManyToManyRelations(ApplicationDbContext dbContext, AdminAssetMod adminDto, StringBuilder auditLogger)
		{
			List<int> playerIds = adminDto.PlayerIds ?? new();
			foreach (PlayerAssetMod newEntity in playerIds.ConvertAll(pi => new PlayerAssetMod { AssetModId = Id, PlayerId = pi }))
			{
				if (!dbContext.PlayerAssetMods.Any(pam => pam.AssetModId == newEntity.AssetModId && pam.PlayerId == newEntity.PlayerId))
					dbContext.PlayerAssetMods.Add(newEntity);
			}

			foreach (PlayerAssetMod entityToRemove in dbContext.PlayerAssetMods.Where(pam => pam.AssetModId == Id && !playerIds.Contains(pam.PlayerId)))
				dbContext.PlayerAssetMods.Remove(entityToRemove);
		}

		public AdminAssetMod Populate()
		{
			return new()
			{
				AssetModTypes = ToFlagEnumList(AssetModTypes).ToList(),
				AssetModFileContents = ToFlagEnumList(AssetModFileContents).ToList(),
				Name = Name,
				PlayerIds = PlayerAssetMods.ConvertAll(pam => pam.PlayerId),
				Url = Url,
			};

			static IEnumerable<T> ToFlagEnumList<T>(T value)
				where T : struct, Enum
				=> Enum.GetValues(typeof(T)).Cast<T>().Where(r => ((int)(object)value & (int)(object)r) == (int)(object)r);
		}
	}
}
