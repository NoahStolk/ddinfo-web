using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevilDaggersWebsite.Entities
{
	public class Player : IAdminUpdatableEntity<AdminPlayer>
	{
		[Key]
		public int Id { get; set; }

		[StringLength(32)]
		public string PlayerName { get; set; } = null!;

		[StringLength(2)]
		public string? CountryCode { get; set; }

		public int? Dpi { get; set; }

		public float? InGameSens { get; set; }

		public int? Fov { get; set; }

		public bool? IsRightHanded { get; set; }

		public bool? HasFlashHandEnabled { get; set; }

		public float? Gamma { get; set; }

		public bool? UsesLegacyAudio { get; set; }

		public bool IsBanned { get; set; }

		[StringLength(64)]
		public string? BanDescription { get; set; }

		public int? BanResponsibleId { get; set; }

		public bool IsBannedFromDdcl { get; set; }

		public bool HideSettings { get; set; }

		public bool HideDonations { get; set; }

		public bool HidePastUsernames { get; set; }

		public List<PlayerAssetMod> PlayerAssetMods { get; set; } = new();

		public List<PlayerTitle> PlayerTitles { get; set; } = new();

		public float? Edpi => Dpi * InGameSens;

		public string EdpiString => Edpi.HasValue ? Edpi.Value.ToString(FormatUtils.InGameSensFormat) : string.Empty;
		public string DpiString => Dpi.HasValue ? Dpi.Value.ToString() : string.Empty;
		public string InGameSensString => InGameSens.HasValue ? InGameSens.Value.ToString(FormatUtils.InGameSensFormat) : string.Empty;
		public string GammaString => Gamma.HasValue ? Gamma.Value.ToString(FormatUtils.GammaFormat) : string.Empty;
		public string RightHandedString => IsRightHanded.HasValue ? IsRightHanded.Value ? "Right" : "Left" : string.Empty;
		public string FlashEnabledString => HasFlashHandEnabled.HasValue ? HasFlashHandEnabled.Value ? "On" : "Off" : string.Empty;
		public string UsesLegacyAudioString => UsesLegacyAudio.HasValue ? UsesLegacyAudio.Value ? "On" : "Off" : string.Empty;

		public override string ToString()
			=> $"{PlayerName} ({Id})";

		public void Create(ApplicationDbContext dbContext, AdminPlayer adminDto)
		{
			Id = adminDto.Id;

			Edit(dbContext, adminDto);

			dbContext.Players.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminPlayer adminDto)
		{
			PlayerName = string.IsNullOrWhiteSpace(adminDto.PlayerName) ? (LeaderboardClient.Instance.GetUserById(Id).Result?.Username ?? string.Empty) : adminDto.PlayerName;
			CountryCode = adminDto.CountryCode;
			Dpi = adminDto.Dpi;
			InGameSens = adminDto.InGameSens;
			Fov = adminDto.Fov;
			IsRightHanded = adminDto.IsRightHanded;
			HasFlashHandEnabled = adminDto.HasFlashHandEnabled;
			Gamma = adminDto.Gamma;
			UsesLegacyAudio = adminDto.UsesLegacyAudio;
			IsBanned = adminDto.IsBanned;
			BanDescription = adminDto.BanDescription;
			BanResponsibleId = adminDto.BanResponsibleId;
			IsBannedFromDdcl = adminDto.IsBannedFromDdcl;
			HideSettings = adminDto.HideSettings;
			HideDonations = adminDto.HideDonations;
			HidePastUsernames = adminDto.HidePastUsernames;
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
				Id = Id,
				PlayerName = PlayerName,
				CountryCode = CountryCode,
				Dpi = Dpi,
				InGameSens = InGameSens,
				Fov = Fov,
				IsRightHanded = IsRightHanded,
				HasFlashHandEnabled = HasFlashHandEnabled,
				Gamma = Gamma,
				UsesLegacyAudio = UsesLegacyAudio,
				IsBanned = IsBanned,
				BanDescription = BanDescription,
				BanResponsibleId = BanResponsibleId,
				IsBannedFromDdcl = IsBannedFromDdcl,
				HideSettings = HideSettings,
				HideDonations = HideDonations,
				HidePastUsernames = HidePastUsernames,
				AssetModIds = PlayerAssetMods.ConvertAll(pam => pam.AssetModId),
				TitleIds = PlayerTitles.ConvertAll(pt => pt.TitleId),
			};
		}

		public bool IsPublicDonator(IEnumerable<Donation> donations)
			=> !HideDonations && donations.Any(d => d.PlayerId == Id && !d.IsRefunded && d.ConvertedEuroCentsReceived > 0);

		public bool HasSettings()
			=> Dpi.HasValue || InGameSens.HasValue || Fov.HasValue || IsRightHanded.HasValue || HasFlashHandEnabled.HasValue || Gamma.HasValue || UsesLegacyAudio.HasValue;
	}
}
