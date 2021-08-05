using DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons.AuditLog;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Donations;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums.Sortings.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers.Admin
{
	[Route("api/admin/donations")]
	[Authorize(Roles = Roles.Admin)]
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<Page<GetDonation>> GetDonations(
			[Range(0, 1000)] int pageIndex = 0,
			[Range(AdminPagingConstants.PageSizeMin, AdminPagingConstants.PageSizeMax)] int pageSize = AdminPagingConstants.PageSizeDefault,
			DonationSorting? sortBy = null,
			bool ascending = false)
		{
			IQueryable<Donation> donationsQuery = _dbContext.Donations
				.AsNoTracking()
				.Include(d => d.Player);

			donationsQuery = sortBy switch
			{
				DonationSorting.Amount => donationsQuery.OrderBy(d => d.Amount, ascending),
				DonationSorting.ConvertedEuroCentsReceived => donationsQuery.OrderBy(d => d.ConvertedEuroCentsReceived, ascending),
				DonationSorting.Currency => donationsQuery.OrderBy(d => d.Currency, ascending),
				DonationSorting.DateReceived => donationsQuery.OrderBy(d => d.DateReceived, ascending),
				DonationSorting.IsRefunded => donationsQuery.OrderBy(d => d.IsRefunded, ascending),
				DonationSorting.Note => donationsQuery.OrderBy(d => d.Note, ascending),
				DonationSorting.PlayerName => donationsQuery.OrderBy(d => d.Player.PlayerName, ascending),
				_ => donationsQuery.OrderBy(d => d.Id, ascending),
			};

			List<Donation> donations = donationsQuery
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetDonation>
			{
				Results = donations.ConvertAll(d => d.ToGetDonation()),
				TotalResults = _dbContext.Donations.Count(),
			};
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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
