using DevilDaggersWebsite.Enumerators;
using System;
using System.Text;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminDonation
	{
		public int PlayerId { get; init; }
		public int Amount { get; init; }
		public Currency Currency { get; init; }
		public int ConvertedEuroCentsReceived { get; init; }
		public DateTime DateReceived { get; init; }
		public string? Note { get; init; }
		public bool IsRefunded { get; init; }

		public override string ToString()
		{
			StringBuilder sb = new("```\n");
			sb.AppendFormat("{0,-30}", nameof(PlayerId)).AppendLine(PlayerId.ToString());
			sb.AppendFormat("{0,-30}", nameof(Amount)).AppendLine(Amount.ToString());
			sb.AppendFormat("{0,-30}", nameof(Currency)).AppendLine(Currency.ToString());
			sb.AppendFormat("{0,-30}", nameof(ConvertedEuroCentsReceived)).AppendLine(ConvertedEuroCentsReceived.ToString());
			sb.AppendFormat("{0,-30}", nameof(DateReceived)).AppendLine(DateReceived.ToString());
			sb.AppendFormat("{0,-30}", nameof(Note)).AppendLine(Note);
			sb.AppendFormat("{0,-30}", nameof(IsRefunded)).AppendLine(IsRefunded.ToString());
			return sb.AppendLine("```").ToString();
		}
	}
}
