﻿using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Donations;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Api
{
	[Route("api/donations/admin")]
	[ApiController]
	public class DonationsController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly AuditLogger _auditLogger;

		public DonationsController(ApplicationDbContext dbContext, AuditLogger auditLogger)
		{
			_dbContext = dbContext;
			_auditLogger = auditLogger;
		}

		[HttpGet]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<Page<GetDonation>> GetDonations([Range(0, 1000)] int pageIndex = 0, [Range(5, 50)] int pageSize = 25, string? sortBy = null, bool ascending = false)
		{
			IQueryable<Donation> donationsQuery = _dbContext.Donations
				.AsNoTracking()
				.Include(d => d.Player);

			if (sortBy != null)
				donationsQuery = donationsQuery.OrderByMember(sortBy, ascending);

			List<Donation> donations = donationsQuery
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetDonation>
			{
				Results = donations.ConvertAll(d => new GetDonation
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
				}),
				TotalResults = _dbContext.Donations.Count(),
			};
		}

		[HttpPost]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult> AddDonation(AddDonation addDonation)
		{
			if (!_dbContext.Players.Any(p => p.Id == addDonation.PlayerId))
				return BadRequest($"Player with ID '{addDonation.PlayerId}' does not exist.");

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

			await _auditLogger.LogAdd(addDonation, User, donation.Id);

			return Ok(donation.Id);
		}

		[HttpPut("{id}")]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult> EditDonationById(int id, EditDonation editDonation)
		{
			if (!_dbContext.Players.Any(p => p.Id == editDonation.PlayerId))
				return BadRequest($"Player with ID '{editDonation.PlayerId}' does not exist.");

			Donation? donation = _dbContext.Donations.FirstOrDefault(d => d.Id == id);
			if (donation == null)
				return NotFound();

			EditDonation logDto = new()
			{
				Amount = donation.Amount,
				ConvertedEuroCentsReceived = donation.ConvertedEuroCentsReceived,
				Currency = donation.Currency,
				DateReceived = donation.DateReceived,
				IsRefunded = donation.IsRefunded,
				Note = donation.Note,
				PlayerId = donation.PlayerId,
			};

			donation.Amount = editDonation.Amount;
			donation.ConvertedEuroCentsReceived = editDonation.ConvertedEuroCentsReceived;
			donation.Currency = editDonation.Currency;
			donation.DateReceived = editDonation.DateReceived;
			donation.IsRefunded = editDonation.IsRefunded;
			donation.Note = editDonation.Note;
			donation.PlayerId = editDonation.PlayerId;
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editDonation, User, donation.Id);

			return Ok();
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult> DeleteDonationById(int id)
		{
			Donation? donation = _dbContext.Donations.FirstOrDefault(d => d.Id == id);
			if (donation == null)
				return NotFound();

			_dbContext.Donations.Remove(donation);
			_dbContext.SaveChanges();

			await _auditLogger.LogDelete(donation, User, donation.Id);

			return Ok();
		}
	}
}
