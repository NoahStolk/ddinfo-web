using DevilDaggersWebsite.Authorization;
using DevilDaggersWebsite.Dto.Donations;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Api
{
	[Route("api/donations")]
	[ApiController]
	public class DonationsController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;

		public DonationsController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		[Authorize(Policies.AdminPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<GetDonationDto>> GetDonations()
		{
			List<Donation> donations = _dbContext.Donations
				.AsNoTracking()
				.Include(d => d.Player)
				.ToList();

			return donations.ConvertAll(d => new GetDonationDto
			{
				Id = d.Id,
				Amount = d.Amount,
				ConvertedEuroCentsReceived = d.ConvertedEuroCentsReceived,
				Currency = d.Currency,
				DateReceived = d.DateReceived,
				IsRefunded = d.IsRefunded,
				Note = d.Note,
				PlayerId = d.PlayerId,
				PlayerName = d.Player.PlayerName,
			});
		}

		[HttpPost]
		[Authorize(Policies.AdminPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
		public ActionResult AddDonation(AddDonationDto addDonation)
		{
			if (!_dbContext.Players.Any(p => p.Id == addDonation.PlayerId))
				return BadRequest($"Player with ID {addDonation.PlayerId} does not exist.");

			Donation donation = new()
			{
				Amount = addDonation.Amount,
				ConvertedEuroCentsReceived = addDonation.ConvertedEuroCentsReceived,
				Currency = addDonation.Currency,
				DateReceived = addDonation.DateReceived,
				IsRefunded = addDonation.IsRefunded,
				Note = addDonation.Note,
				PlayerId = addDonation.PlayerId,
			};
			_dbContext.Donations.Add(donation);
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpPut("{id}")]
		[Authorize(Policies.AdminPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult EditTitle(int id, EditDonationDto editDonation)
		{
			if (!_dbContext.Players.Any(p => p.Id == editDonation.PlayerId))
				return BadRequest($"Player with ID {editDonation.PlayerId} does not exist.");

			Donation? donation = _dbContext.Donations.FirstOrDefault(d => d.Id == id);
			if (donation == null)
				return NotFound();

			donation.Amount = editDonation.Amount;
			donation.ConvertedEuroCentsReceived = editDonation.ConvertedEuroCentsReceived;
			donation.Currency = editDonation.Currency;
			donation.DateReceived = editDonation.DateReceived;
			donation.IsRefunded = editDonation.IsRefunded;
			donation.Note = editDonation.Note;
			donation.PlayerId = editDonation.PlayerId;
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpDelete("{id}")]
		[Authorize(Policies.AdminPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult DeleteTitle(int id)
		{
			Donation? donation = _dbContext.Donations.FirstOrDefault(d => d.Id == id);
			if (donation == null)
				return NotFound();

			_dbContext.Donations.Remove(donation);
			_dbContext.SaveChanges();

			return Ok();
		}
	}
}
