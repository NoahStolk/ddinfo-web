namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("Donations")]
public class DonationEntity
{
	[Key]
	public int Id { get; init; }

	public int PlayerId { get; set; }

	[ForeignKey(nameof(PlayerId))]
	public PlayerEntity Player { get; set; } = null!;

	public int Amount { get; set; }

	public Currency Currency { get; set; }

	public int ConvertedEuroCentsReceived { get; set; }

	public DateTime DateReceived { get; set; }

	[StringLength(64)]
	public string? Note { get; set; }

	public bool IsRefunded { get; set; }
}
