using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.Authorization;
using DevilDaggersWebsite.Dto.Titles;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Api
{
	[Route("api/titles")]
	[ApiController]
	public class TitlesController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;

		public TitlesController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		[Authorize(Policies.AdminPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<List<GetTitle>> GetTitles()
		{
			List<Title> titles = _dbContext.Titles
				.AsNoTracking()
				.Include(t => t.PlayerTitles)
				.ToList();

			return titles.ConvertAll(t => new GetTitle
			{
				Id = t.Id,
				Name = t.Name,
				PlayerIds = t.PlayerTitles.ConvertAll(pt => pt.PlayerId),
			});
		}

		[HttpPost]
		[Authorize(Policies.AdminPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult AddTitle(AddTitle addTitle)
		{
			foreach (int playerId in addTitle.PlayerIds ?? new())
			{
				if (!_dbContext.Players.Any(p => p.Id == playerId))
					return BadRequest($"Player with ID {playerId} does not exist.");
			}

			Title title = new()
			{
				Name = addTitle.Name,
			};
			_dbContext.Titles.Add(title);
			_dbContext.SaveChanges(); // Save changes here so PlayerTitle entities can be assigned properly.

			UpdateManyToManyRelations(addTitle.PlayerIds ?? new(), title.Id);
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpPut("{id}")]
		[Authorize(Policies.AdminPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult EditTitle(int id, EditTitle editTitle)
		{
			foreach (int playerId in editTitle.PlayerIds ?? new())
			{
				if (!_dbContext.Players.Any(p => p.Id == playerId))
					return BadRequest($"Player with ID {playerId} does not exist.");
			}

			Title? title = _dbContext.Titles.FirstOrDefault(t => t.Id == id);
			if (title == null)
				return NotFound();

			title.Name = editTitle.Name;

			UpdateManyToManyRelations(editTitle.PlayerIds ?? new(), title.Id);
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpDelete("{id}")]
		[Authorize(Policies.AdminPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult DeleteTitle(int id)
		{
			Title? title = _dbContext.Titles
				.Include(t => t.PlayerTitles)
				.FirstOrDefault(t => t.Id == id);
			if (title == null)
				return NotFound();

			_dbContext.Titles.Remove(title);
			_dbContext.SaveChanges();

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
