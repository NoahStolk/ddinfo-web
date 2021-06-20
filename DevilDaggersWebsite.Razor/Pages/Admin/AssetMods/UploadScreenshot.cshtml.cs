using DevilDaggersDiscordBot;
using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class UploadScreenshotModel : PageModel
	{
		public const int MaxFileSize = 1024 * 1024;
		public const int MaxFileNameLength = 50;

		private readonly IWebHostEnvironment _env;
		private readonly ApplicationDbContext _dbContext;

		public UploadScreenshotModel(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			_env = env;
			_dbContext = dbContext;

			ModNames = _dbContext.AssetMods.Select(am => new SelectListItem(am.Name, am.Name)).ToList();
		}

		public static int MaxScreenshots { get; } = 10;

		[BindProperty]
		public IFormFile? FormFile { get; set; }

		[BindProperty]
		public string? ModName { get; set; }

		public List<SelectListItem> ModNames { get; }

		public async Task OnPostAsync()
		{
			string failedAttemptMessage = $":x: Failed attempt from `{this.GetIdentity()}` to upload new ASSETMOD screenshot";

			try
			{
				if (string.IsNullOrEmpty(ModName))
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: No mod selected.");
					return;
				}

				if (FormFile == null)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: No file.");
					return;
				}

				if (FormFile.Length > MaxFileSize)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File too large (`{FormFile.Length:n0}` / max `{MaxFileSize:n0}` bytes).");
					return;
				}

				if (FormFile.FileName.Length > MaxFileNameLength)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File name too long (`{FormFile.FileName.Length}` / max `{MaxFileNameLength}` characters).");
					return;
				}

				if (!FormFile.FileName.EndsWith(".png"))
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File name must have the `.png` extension.");
					return;
				}

				string fileDirectory = Path.Combine(_env.WebRootPath, "mod-screenshots", ModName);
				string filePath = Path.Combine(fileDirectory, FormFile.FileName);
				if (Io.File.Exists(filePath))
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File `{FormFile.FileName}` already exists.");
					return;
				}

				if (!_dbContext.AssetMods.Any(am => am.Name == ModName))
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: Mod `{ModName}` does not exist.");
					return;
				}

				if (Directory.Exists(fileDirectory) && Directory.GetFiles(fileDirectory).Length >= MaxScreenshots)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: Mod `{ModName}` already has `{MaxScreenshots}` screenshots.");
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

				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $":white_check_mark: `{this.GetIdentity()}` uploaded new ASSETMOD screenshot :frame_photo: `{FormFile.FileName}` for mod `{ModName}` (`{formFileBytes.Length:n0}` bytes)");
			}
			catch (Exception ex)
			{
				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: Fatal error: `{ex.Message}`");
			}
		}
	}
}
