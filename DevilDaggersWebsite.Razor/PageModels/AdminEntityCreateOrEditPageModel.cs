using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityCreateOrEditPageModel<TEntity, TAdminDto> : AbstractAdminEntityPageModel<TEntity>
		where TEntity : class, IAdminUpdatableEntity<TAdminDto>, new()
		where TAdminDto : class, IAdminDto
	{
		private TEntity? _entity;

		public AdminEntityCreateOrEditPageModel(ApplicationDbContext dbContext, IWebHostEnvironment environment)
			: base(dbContext, environment)
		{
			AssetModTypesList = RazorUtils.EnumToSelectList<AssetModTypes>(true);

			CategoryList = RazorUtils.EnumToSelectList<CustomLeaderboardCategory>(true);
			CurrencyList = RazorUtils.EnumToSelectList<Currency>(false);

			AssetModList = DbContext.AssetMods.Select(am => new SelectListItem(am.Name, am.Id.ToString())).ToList();
			CustomLeaderboardList = DbContext.CustomLeaderboards.Include(cl => cl.SpawnsetFile).Select(cl => new SelectListItem(cl.SpawnsetFile.Name, cl.Id.ToString())).ToList();
			PlayerList = DbContext.Players.Select(p => new SelectListItem(p.PlayerName, p.Id.ToString())).ToList();
			SpawnsetFileList = DbContext.SpawnsetFiles.Select(sf => new SelectListItem(sf.Name, sf.Id.ToString())).ToList();
			TitleList = DbContext.Titles.Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList();
		}

		public List<SelectListItem> AssetModTypesList { get; }

		public List<SelectListItem> CategoryList { get; }
		public List<SelectListItem> CurrencyList { get; }

		public List<SelectListItem> AssetModList { get; }
		public List<SelectListItem> CustomLeaderboardList { get; }
		public List<SelectListItem> PlayerList { get; }
		public List<SelectListItem> SpawnsetFileList { get; }
		public List<SelectListItem> TitleList { get; }

		public int? Id { get; private set; }

		[BindProperty]
		public TAdminDto AdminDto { get; set; } = null!;

		public bool IsEditing => Id.HasValue;

		public IActionResult OnGet(int? id)
		{
			Id = id;

			_entity = GetFullQuery().FirstOrDefault(m => m.Id == id) ?? new();

			AdminDto = _entity.Populate();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (!ModelState.IsValid)
				return Page();

			if (AdminDto == null)
				throw new($"{nameof(AdminDto)} should not be null on POST.");

			Id = id;

			AdminAssetMod? assetMod = AdminDto as AdminAssetMod;
			AdminCustomLeaderboard? customLeaderboard = AdminDto as AdminCustomLeaderboard;
			AdminPlayer? player = AdminDto as AdminPlayer;
			AdminSpawnsetFile? spawnsetFile = AdminDto as AdminSpawnsetFile;

			if (assetMod?.ValidateGlobal(ModelState) == false)
				return Page();

			if (customLeaderboard?.ValidateGlobal(ModelState, DbContext, Environment) == false)
				return Page();

			if (player?.ValidateGlobal(ModelState) == false)
				return Page();

			if (IsEditing)
			{
				_entity = GetFullQuery().FirstOrDefault(m => m.Id == id);
				if (_entity == null)
					return NotFound();

				DbContext.Attach(_entity).State = EntityState.Modified;

				try
				{
					StringBuilder auditLogger = new($"`EDIT` by `{GetIdentity()}` for `{typeof(TEntity).Name}` `{id}`\n");
					LogCreateOrEdit(auditLogger, _entity.Populate().Log(), AdminDto.Log());

					_entity.Edit(DbContext, AdminDto);
					DbContext.SaveChanges();

					_entity.CreateManyToManyRelations(DbContext, AdminDto);
					DbContext.SaveChanges();

					await DiscordLogger.TryLog(LoggingChannel, Environment.EnvironmentName, auditLogger.ToString());
				}
				catch (DbUpdateConcurrencyException) when (!DbSet.Any(e => e.Id == _entity.Id))
				{
					return NotFound();
				}
			}
			else
			{
				if (assetMod != null && DbContext.AssetMods.Any(am => am.Name == assetMod.Name))
				{
					ModelState.AddModelError($"AdminDto.{nameof(AdminAssetMod.Name)}", $"AssetMod with {nameof(AssetMod.Name)} '{assetMod.Name}' already exists.");
					return Page();
				}

				if (customLeaderboard != null && DbContext.CustomLeaderboards.Any(cl => cl.SpawnsetFileId == customLeaderboard.SpawnsetFileId))
				{
					ModelState.AddModelError($"AdminDto.{nameof(AdminCustomLeaderboard.SpawnsetFileId)}", "A leaderboard for this spawnset already exists.");
					return Page();
				}

				if (player != null && DbContext.Players.Any(p => p.Id == player.Id))
				{
					ModelState.AddModelError($"AdminDto.{nameof(AdminPlayer.Id)}", $"Player with {nameof(AdminPlayer.Id)} '{player.Id}' already exists.");
					return Page();
				}

				if (spawnsetFile != null && DbContext.SpawnsetFiles.Any(sf => sf.Name == spawnsetFile.Name))
				{
					ModelState.AddModelError($"AdminDto.{nameof(AdminSpawnsetFile.Name)}", $"SpawnsetFile with {nameof(AdminSpawnsetFile.Name)} '{spawnsetFile.Name}' already exists.");
					return Page();
				}

				_entity = new();
				_entity.Create(DbContext, AdminDto);
				DbContext.SaveChanges();

				_entity.CreateManyToManyRelations(DbContext, AdminDto);
				DbContext.SaveChanges();

				StringBuilder auditLogger = new($"`CREATE` by `{GetIdentity()}` for `{typeof(TEntity).Name}` `{_entity.Id}`\n");
				LogCreateOrEdit(auditLogger, null, AdminDto.Log());

				await DiscordLogger.TryLog(LoggingChannel, Environment.EnvironmentName, auditLogger.ToString());
			}

			return RedirectToPage("./Index");
		}

		private IQueryable<TEntity> GetFullQuery()
		{
			if (typeof(TEntity) == typeof(Title))
				return DbSet.Include(t => (t as Title)!.PlayerTitles);
			if (typeof(TEntity) == typeof(AssetMod))
				return DbSet.Include(t => (t as AssetMod)!.PlayerAssetMods);
			if (typeof(TEntity) == typeof(Player))
				return DbSet.Include(t => (t as Player)!.PlayerAssetMods).Include(t => (t as Player)!.PlayerTitles);
			return DbSet.AsQueryable();
		}
	}
}
