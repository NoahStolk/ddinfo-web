using System.Collections.Generic;
using System.Text;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminPlayer
	{
		public int Id { get; init; }
		public string PlayerName { get; init; } = null!;
		public bool IsAnonymous { get; init; }
		public List<int>? AssetModIds { get; init; }
		public List<int>? TitleIds { get; init; }
		public string? CountryCode { get; init; }
		public int? Dpi { get; init; }
		public float? InGameSens { get; init; }
		public int? Fov { get; init; }
		public bool? RightHanded { get; init; }
		public bool? FlashEnabled { get; init; }
		public float? Gamma { get; init; }
		public bool IsBanned { get; init; }
		public string? BanDescription { get; init; }
		public int? BanResponsibleId { get; init; }
		public bool? UsesLegacyAudio { get; init; }
		public bool IsBannedFromDdcl { get; init; }

		public override string ToString()
		{
			StringBuilder sb = new();
			sb.AppendFormat("{0,-22}", nameof(Id)).AppendLine(Id.ToString());
			sb.AppendFormat("{0,-22}", nameof(PlayerName)).AppendLine(PlayerName);
			sb.AppendFormat("{0,-22}", nameof(IsAnonymous)).AppendLine(IsAnonymous.ToString());
			if (AssetModIds != null)
				sb.AppendFormat("{0,-22}", nameof(AssetModIds)).AppendLine(string.Join(", ", AssetModIds));
			if (TitleIds != null)
				sb.AppendFormat("{0,-22}", nameof(TitleIds)).AppendLine(string.Join(", ", TitleIds));
			sb.AppendFormat("{0,-22}", nameof(CountryCode)).AppendLine(CountryCode);
			sb.AppendFormat("{0,-22}", nameof(Dpi)).AppendLine(Dpi.ToString());
			sb.AppendFormat("{0,-22}", nameof(InGameSens)).AppendLine(InGameSens.ToString());
			sb.AppendFormat("{0,-22}", nameof(Fov)).AppendLine(Fov.ToString());
			sb.AppendFormat("{0,-22}", nameof(RightHanded)).AppendLine(RightHanded.ToString());
			sb.AppendFormat("{0,-22}", nameof(FlashEnabled)).AppendLine(FlashEnabled.ToString());
			sb.AppendFormat("{0,-22}", nameof(Gamma)).AppendLine(Gamma.ToString());
			sb.AppendFormat("{0,-22}", nameof(IsBanned)).AppendLine(IsBanned.ToString());
			sb.AppendFormat("{0,-22}", nameof(BanDescription)).AppendLine(BanDescription);
			sb.AppendFormat("{0,-22}", nameof(BanResponsibleId)).AppendLine(BanResponsibleId.ToString());
			sb.AppendFormat("{0,-22}", nameof(UsesLegacyAudio)).AppendLine(UsesLegacyAudio.ToString());
			sb.AppendFormat("{0,-22}", nameof(IsBannedFromDdcl)).AppendLine(IsBannedFromDdcl.ToString());
			return sb.ToString();
		}
	}
}
