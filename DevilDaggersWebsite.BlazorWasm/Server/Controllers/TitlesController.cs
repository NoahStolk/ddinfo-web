using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Titles;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Api
{
	[Route("api/titles/admin")]
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
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<Page<GetTitle>> GetTitles()
		{
			List<Title> titles = _dbContext.Titles
				.AsNoTracking()
				.Include(t => t.PlayerTitles)
				.ToList();

			return new Page<GetTitle>
			{
				Results = titles.ConvertAll(t => new GetTitle
				{
					Id = t.Id,
					Name = t.Name,
					PlayerIds = t.PlayerTitles.ConvertAll(pt => pt.PlayerId),
				}),
				TotalResults = _dbContext.Titles.Count(),
			};
		}

		[HttpPost]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
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

			UpdateManyToManyRelations(addTitle.PlayerIds ?? new(), title.Id);
			_dbContext.SaveChanges();

			await _auditLogger.LogAdd(addTitle, User, title.Id);

			return Ok(title.Id);
		}

		[HttpPut("{id}")]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
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

			UpdateManyToManyRelations(editTitle.PlayerIds ?? new(), title.Id);
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editTitle, User, title.Id);

			return Ok();
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
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

		private void UpdateManyToManyRelations(List<int> playerIds, int titleId)
		{
			foreach (PlayerTitle newEntity in playerIds.ConvertAll(pi => new PlayerTitle { TitleId = titleId, PlayerId = pi }))
			{
				if (!_dbContext.PlayerTitles.Any(pam => pam.TitleId == newEntity.TitleId && pam.PlayerId == newEntity.PlayerId))
					_dbContext.PlayerTitles.Add(newEntity);
			}

			foreach (PlayerTitle entityToRemove in _dbContext.PlayerTitles.Where(pam => pam.TitleId == titleId && !playerIds.Contains(pam.PlayerId)))
				_dbContext.PlayerTitles.Remove(entityToRemove);
		}
	}
}
