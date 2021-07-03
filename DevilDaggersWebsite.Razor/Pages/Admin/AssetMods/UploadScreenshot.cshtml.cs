using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class UploadScreenshotModel : AbstractAdminPageModel
	{
		public const int MaxFileSize = 1024 * 1024;
		public const int MaxFileNameLength = 50;

		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly DiscordLogger _discordLogger;

		public UploadScreenshotModel(IWebHostEnvironment environment, ApplicationDbContext dbContext, DiscordLogger discordLogger)
		{
			_environment = environment;
			_dbContext = dbContext;
			_discordLogger = discordLogger;

			ModNames = _dbContext.AssetMods
				.AsNoTracking()
				.Select(am => am.Name)
				.ToList()
				.ConvertAll(n => new SelectListItem(n, n));
		}

		public static int MaxScreenshots { get; } = 10;

		[BindProperty]
		public IFormFile? FormFile { get; set; }

		[BindProperty]
		public string? ModName { get; set; }

		public List<SelectListItem> ModNames { get; }

		public async Task OnPostAsync()
		{
			string failedAttemptMessage = $":x: Failed attempt from `{GetIdentity()}` to upload new ASSETMOD screenshot";

			try
			{
				if (string.IsNullOrEmpty(ModName))
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: No mod selected.");
					return;
				}

				if (FormFile == null)
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: No file.");
					return;
				}

				if (FormFile.Length > MaxFileSize)
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: File too large (`{FormFile.Length:n0}` / max `{MaxFileSize:n0}` bytes).");
					return;
				}

				if (FormFile.FileName.Length > MaxFileNameLength)
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: File name too long (`{FormFile.FileName.Length}` / max `{MaxFileNameLength}` characters).");
					return;
				}

				if (!FormFile.FileName.EndsWith(".png"))
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: File name must have the `.png` extension.");
					return;
				}

				string fileDirectory = Path.Combine(_environment.WebRootPath, "mod-screenshots", ModName);
				string filePath = Path.Combine(fileDirectory, FormFile.FileName);
				if (Io.File.Exists(filePath))
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: File `{FormFile.FileName}` already exists.");
					return;
				}

				if (!_dbContext.AssetMods.Any(am => am.Name == ModName))
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: Mod `{ModName}` does not exist.");
					return;
				}

				if (Directory.Exists(fileDirectory) && Directory.GetFiles(fileDirectory).Length >= MaxScreenshots)
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: Mod `{ModName}` already has `{MaxScreenshots}` screenshots.");
					return;
				}

				byte[] formFileBytes = new byte[FormFile.Length];
				using (MemoryStream ms = new())
				{
					FormFile.CopyTo(ms);
					formFileBytes = ms.ToArray();
				}

				Directory.CreateDirectory(fileDirectory);
				Io.File.WriteAllBytes(filePath, formFileBytes);

				await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $":white_check_mark: `{GetIdentity()}` uploaded new ASSETMOD screenshot :frame_photo: `{FormFile.FileName}` for mod `{ModName}` (`{formFileBytes.Length:n0}` bytes)");
			}
			catch (Exception ex)
			{
				await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: Fatal error: `{ex.Message}`");
			}
		}
	}
}
