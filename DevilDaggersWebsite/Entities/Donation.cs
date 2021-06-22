using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Enumerators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class Donation : IAdminUpdatableEntity<AdminDonation>
	{
		[Key]
		public int Id { get; init; }

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; } = null!;

		public int Amount { get; set; }

		public Currency Currency { get; set; }

		public int ConvertedEuroCentsReceived { get; set; }

		public DateTime DateReceived { get; set; }

		[StringLength(64)]
		public string? Note { get; set; }

		public bool IsRefunded { get; set; }

		public void Create(ApplicationDbContext dbContext, AdminDonation adminDto)
		{
			Edit(dbContext, adminDto);

			dbContext.Donations.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminDonation adminDto)
		{
			PlayerId = adminDto.PlayerId;
			Amount = adminDto.Amount;
			Currency = adminDto.Currency;
			ConvertedEuroCentsReceived = adminDto.ConvertedEuroCentsReceived;
			DateReceived = adminDto.DateReceived;
			Note = adminDto.Note;
			IsRefunded = adminDto.IsRefunded;
		}

		public AdminDonation Populate()
		{
			return new()
			{
				PlayerId = PlayerId,
				Amount = Amount,
				Currency = Currency,
				ConvertedEuroCentsReceived = ConvertedEuroCentsReceived,
				DateReceived = DateReceived,
				Note = Note,
				IsRefunded = IsRefunded,
			};
		}
	}
}
