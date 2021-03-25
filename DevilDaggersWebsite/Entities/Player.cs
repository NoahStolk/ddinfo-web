using DevilDaggersWebsite.Dto.Admin;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DevilDaggersWebsite.Entities
{
	public class Player : IAdminUpdatableEntity<AdminPlayer>
	{
		[Key]
		public int Id { get; set; }

		public string PlayerName { get; set; } = null!;
		public bool IsAnonymous { get; set; }
		public List<PlayerAssetMod> PlayerAssetMods { get; set; } = new();
		public List<PlayerTitle> PlayerTitles { get; set; } = new();
		public string? CountryCode { get; set; }
		public int? Dpi { get; set; }
		public float? InGameSens { get; set; }
		public int? Fov { get; set; }
		public bool? RightHanded { get; set; }
		public bool? FlashEnabled { get; set; }
		public float? Gamma { get; set; }
		public bool IsBanned { get; set; }
		public string? BanDescription { get; set; }
		public int? BanResponsibleId { get; set; }
		public bool? UsesLegacyAudio { get; set; }
		public bool IsBannedFromDdcl { get; set; }

		public float? Edpi => Dpi * InGameSens;
		public string RightHandedString => !RightHanded.HasValue ? string.Empty : RightHanded.Value ? "Right" : "Left";
		public string FlashEnabledString => !FlashEnabled.HasValue ? string.Empty : FlashEnabled.Value ? "On" : "Off";
		public string UsesLegacyAudioString => !UsesLegacyAudio.HasValue ? string.Empty : UsesLegacyAudio.Value ? "On" : "Off";

		public override string ToString()
			=> $"{PlayerName} ({Id})";

		public void Create(ApplicationDbContext dbContext, AdminPlayer adminDto, StringBuilder auditLogger)
		{
			Id = adminDto.Id;

			auditLogger.AppendFormat("{0,20}", "Id").Append(": ").AppendFormat("{0,20}", string.Empty).Append(" -> ").AppendFormat("{0,20}", Id).AppendLine();

			Edit(dbContext, adminDto, auditLogger);

			dbContext.Players.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminPlayer adminDto, StringBuilder auditLogger)
		{
			(this as IAdminUpdatableEntity<AdminPlayer>).TrackEditUpdates(auditLogger, adminDto, typeof(Player));

			PlayerName = adminDto.PlayerName;
			IsAnonymous = adminDto.IsAnonymous;
			CountryCode = adminDto.CountryCode;
			Dpi = adminDto.Dpi;
			InGameSens = adminDto.InGameSens;
			Fov = adminDto.Fov;
			RightHanded = adminDto.RightHanded;
			FlashEnabled = adminDto.FlashEnabled;
			Gamma = adminDto.Gamma;
			IsBanned = adminDto.IsBanned;
			BanDescription = adminDto.BanDescription;
			BanResponsibleId = adminDto.BanResponsibleId;
			UsesLegacyAudio = adminDto.UsesLegacyAudio;
			IsBannedFromDdcl = adminDto.IsBannedFromDdcl;
		}

		public void CreateManyToManyRelations(ApplicationDbContext dbContext, AdminPlayer adminDto)
		{
			List<int> assetModIds = adminDto.AssetModIds ?? new();
			foreach (PlayerAssetMod newEntity in assetModIds.ConvertAll(ami => new PlayerAssetMod { AssetModId = ami, PlayerId = Id }))
			{
				if (!dbContext.PlayerAssetMods.Any(pam => pam.AssetModId == newEntity.AssetModId && pam.PlayerId == newEntity.PlayerId))
					dbContext.PlayerAssetMods.Add(newEntity);
			}

			foreach (PlayerAssetMod entityToRemove in dbContext.PlayerAssetMods.Where(pam => pam.PlayerId == Id && !assetModIds.Contains(pam.AssetModId)))
				dbContext.PlayerAssetMods.Remove(entityToRemove);

			List<int> titleIds = adminDto.TitleIds ?? new();
			foreach (PlayerTitle newEntity in titleIds.ConvertAll(ti => new PlayerTitle { TitleId = ti, PlayerId = Id }))
			{
				if (!dbContext.PlayerTitles.Any(pam => pam.TitleId == newEntity.TitleId && pam.PlayerId == newEntity.PlayerId))
					dbContext.PlayerTitles.Add(newEntity);
			}

			foreach (PlayerTitle entityToRemove in dbContext.PlayerTitles.Where(pam => pam.PlayerId == Id && !titleIds.Contains(pam.TitleId)))
				dbContext.PlayerTitles.Remove(entityToRemove);
		}

		public AdminPlayer Populate()
		{
			return new()
			{
				AssetModIds = PlayerAssetMods.ConvertAll(pam => pam.AssetModId),
				BanDescription = BanDescription,
				BanResponsibleId = BanResponsibleId,
				CountryCode = CountryCode,
				Dpi = Dpi,
				FlashEnabled = FlashEnabled,
				Fov = Fov,
				Gamma = Gamma,
				Id = Id,
				InGameSens = InGameSens,
				IsAnonymous = IsAnonymous,
				IsBanned = IsBanned,
				PlayerName = PlayerName,
				RightHanded = RightHanded,
				TitleIds = PlayerTitles.ConvertAll(pt => pt.TitleId),
				IsBannedFromDdcl = IsBannedFromDdcl,
				UsesLegacyAudio = UsesLegacyAudio,
			};
		}
	}
}
