using DevilDaggersWebsite.Dto.Admin;
using DevilDaggersWebsite.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevilDaggersWebsite.Entities
{
	public class AssetMod : IAdminUpdatableEntity<AdminAssetMod>
	{
		[Key]
		public int Id { get; set; }

		public List<PlayerAssetMod> PlayerAssetMods { get; set; } = new();

		public AssetModTypes AssetModTypes { get; set; }

		[StringLength(64)]
		public string Name { get; set; } = null!;

		[StringLength(128)]
		public string Url { get; set; } = null!;

		public bool IsHidden { get; set; }

		public DateTime LastUpdated { get; set; }

		[StringLength(64)]
		public string? TrailerUrl { get; set; }

		[StringLength(2048)]
		public string? HtmlDescription { get; set; }

		public void Create(ApplicationDbContext dbContext, AdminAssetMod adminDto)
		{
			Edit(dbContext, adminDto);

			dbContext.AssetMods.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminAssetMod adminDto)
		{
			AssetModTypes = adminDto.AssetModTypes == null ? AssetModTypes.None : (AssetModTypes)adminDto.AssetModTypes.Cast<int>().Sum();
			Name = adminDto.Name;
			Url = adminDto.Url ?? string.Empty;
			IsHidden = adminDto.IsHidden;
			LastUpdated = adminDto.LastUpdated;
			TrailerUrl = adminDto.TrailerUrl;
			HtmlDescription = adminDto.HtmlDescription;
		}

		public void CreateManyToManyRelations(ApplicationDbContext dbContext, AdminAssetMod adminDto)
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
				Name = Name,
				PlayerIds = PlayerAssetMods.ConvertAll(pam => pam.PlayerId),
				Url = Url,
				IsHidden = IsHidden,
				LastUpdated = LastUpdated,
				TrailerUrl = TrailerUrl,
				HtmlDescription = HtmlDescription,
			};

			static IEnumerable<T> ToFlagEnumList<T>(T value)
				where T : struct, Enum
				=> Enum.GetValues(typeof(T)).Cast<T>().Where(r => ((int)(object)value & (int)(object)r) == (int)(object)r);
		}
	}
}
