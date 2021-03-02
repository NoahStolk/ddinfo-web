using DevilDaggersWebsite.Dto.Admin;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

		public float? Edpi => Dpi * InGameSens;
		public string RightHandedString => !RightHanded.HasValue ? string.Empty : RightHanded.Value ? "Right" : "Left";
		public string FlashEnabledString => !FlashEnabled.HasValue ? string.Empty : FlashEnabled.Value ? "On" : "Off";

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

			dbContext.PlayerAssetMods.RemoveRange(PlayerAssetMods);
			dbContext.PlayerAssetMods.AddRange(adminDto.AssetModIds.ConvertAll(ami => new PlayerAssetMod { AssetModId = ami, PlayerId = Id }));

			dbContext.PlayerTitles.RemoveRange(PlayerTitles);
			dbContext.PlayerTitles.AddRange(adminDto.TitleIds.ConvertAll(ti => new PlayerTitle { PlayerId = Id, TitleId = ti }));
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
			};
		}
	}
}
