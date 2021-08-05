﻿using DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons.AuditLog;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Titles;
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
	[Route("api/admin/titles")]
	[Authorize(Roles = Roles.Admin)]
	[ApiController]
	public class TitlesController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly AuditLogger _auditLogger;

		public TitlesController(ApplicationDbContext dbContext, AuditLogger auditLogger)
		{
			_dbContext = dbContext;
			_auditLogger = auditLogger;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<Page<GetTitle>> GetTitles(
			[Range(0, 1000)] int pageIndex = 0,
			[Range(AdminPagingConstants.PageSizeMin, AdminPagingConstants.PageSizeMax)] int pageSize = AdminPagingConstants.PageSizeDefault,
			string? sortBy = null,
			bool ascending = true)
		{
			IQueryable<Title> titlesQuery = _dbContext.Titles
				.AsNoTracking()
				.Include(t => t.PlayerTitles);

			if (sortBy != null)
				titlesQuery = titlesQuery.OrderByMember(sortBy, ascending);

			List<Title> titles = titlesQuery
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetTitle>
			{
				Results = titles.ConvertAll(t => t.ToGetTitle()),
				TotalResults = _dbContext.Titles.Count(),
			};
		}

		[HttpGet("names")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<GetTitleName>> GetTitleNames()
		{
			var titles = _dbContext.Titles
				.AsNoTracking()
				.Select(t => new { t.Id, t.Name })
				.ToList();

			return titles.ConvertAll(p => new GetTitleName
			{
				Id = p.Id,
				Name = p.Name,
			});
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> AddTitle(AddTitle addTitle)
		{
			foreach (int playerId in addTitle.PlayerIds ?? new())
			{
				if (!_dbContext.Players.Any(p => p.Id == playerId))
					return BadRequest($"Player with ID '{playerId}' does not exist.");
			}

			Title title = new()
			{
				Name = addTitle.Name,
			};
			_dbContext.Titles.Add(title);
			_dbContext.SaveChanges(); // Save changes here so PlayerTitle entities can be assigned properly.

			UpdatePlayerTitles(addTitle.PlayerIds ?? new(), title.Id);
			_dbContext.SaveChanges();

			await _auditLogger.LogAdd(addTitle, User, title.Id);

			return Ok(title.Id);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> EditTitleById(int id, EditTitle editTitle)
		{
			foreach (int playerId in editTitle.PlayerIds ?? new())
			{
				if (!_dbContext.Players.Any(p => p.Id == playerId))
					return BadRequest($"Player with ID '{playerId}' does not exist.");
			}

			Title? title = _dbContext.Titles
				.Include(t => t.PlayerTitles)
				.FirstOrDefault(t => t.Id == id);
			if (title == null)
				return NotFound();

			EditTitle logDto = new()
			{
				Name = title.Name,
				PlayerIds = title.PlayerTitles.ConvertAll(pt => pt.PlayerId),
			};

			title.Name = editTitle.Name;

			UpdatePlayerTitles(editTitle.PlayerIds ?? new(), title.Id);
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editTitle, User, title.Id);

			return Ok();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteTitleById(int id)
		{
			Title? title = _dbContext.Titles
				.Include(t => t.PlayerTitles)
				.FirstOrDefault(t => t.Id == id);
			if (title == null)
				return NotFound();

			_dbContext.Titles.Remove(title);
			_dbContext.SaveChanges();

			await _auditLogger.LogDelete(title, User, title.Id);

			return Ok();
		}

		private void UpdatePlayerTitles(List<int> playerIds, int titleId)
		{
			foreach (PlayerTitle newEntity in playerIds.ConvertAll(pi => new PlayerTitle { TitleId = titleId, PlayerId = pi }))
			{
				if (!_dbContext.PlayerTitles.Any(pt => pt.TitleId == newEntity.TitleId && pt.PlayerId == newEntity.PlayerId))
					_dbContext.PlayerTitles.Add(newEntity);
			}

			foreach (PlayerTitle entityToRemove in _dbContext.PlayerTitles.Where(pt => pt.TitleId == titleId && !playerIds.Contains(pt.PlayerId)))
				_dbContext.PlayerTitles.Remove(entityToRemove);
		}
	}
}
