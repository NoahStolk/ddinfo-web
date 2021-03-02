using DevilDaggersWebsite.Dto.Admin;
using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

		public void Create(ApplicationDbContext dbContext, AdminAssetMod adminDto)
		{
			Edit(dbContext, adminDto);

			dbContext.AssetMods.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminAssetMod adminDto)
		{
			AssetModTypes = adminDto.AssetModTypes;
			AssetModFileContents = adminDto.AssetModFileContents;
			Name = adminDto.Name;
			Url = adminDto.Url;

			dbContext.PlayerAssetMods.RemoveRange(PlayerAssetMods);
			dbContext.PlayerAssetMods.AddRange(adminDto.PlayerIds.ConvertAll(pi => new PlayerAssetMod { AssetModId = Id, PlayerId = pi }));
		}

		public AdminAssetMod Populate()
		{
			return new()
			{
				AssetModTypes = AssetModTypes,
				AssetModFileContents = AssetModFileContents,
				Name = Name,
				PlayerIds = PlayerAssetMods.ConvertAll(pam => pam.PlayerId),
				Url = Url,
			};
		}
	}
}
