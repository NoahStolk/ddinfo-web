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
		private readonly IWebHostEnvironment _env;
		private readonly ApplicationDbContext _dbContext;

		public UploadScreenshotModel(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			_env = env;
			_dbContext = dbContext;

			ModNames = _dbContext.AssetMods.Select(am => new SelectListItem(am.Name, am.Name)).ToList();
		}

		[BindProperty]
		public IFormFile? FormFile { get; set; }

		[BindProperty]
		public string? ModName { get; set; }

		public List<SelectListItem> ModNames { get; }

		public async Task OnPostAsync()
		{
			string failedAttemptMessage = $"Failed attempt from `{this.GetIdentity()}` to upload new ASSETMOD screenshot";

			try
			{
				if (string.IsNullOrEmpty(ModName))
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: No mod selected.");
					return;
				}

				const int maxFileSize = 1024 * 1024;
				const int maxFileNameLength = 30;

				if (FormFile == null)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: No file.");
					return;
				}

				if (FormFile.Length > maxFileSize)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File too large (`{FormFile.Length:n0}` / max `{maxFileSize:n0}` bytes).");
					return;
				}

				if (FormFile.FileName.Length > maxFileNameLength)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File name too long (`{FormFile.FileName.Length}` / max `{maxFileNameLength}` characters).");
					return;
				}

				if (!FormFile.FileName.EndsWith(".png"))
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File name must have the '.png' extension.");
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

				if (Directory.Exists(fileDirectory) && Directory.GetFiles(fileDirectory).Length >= 5)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: Mod `{ModName}` already has 5 screenshots.");
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

				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"`{this.GetIdentity()}` uploaded new ASSETMOD screenshot `{FormFile.FileName}` for mod `{ModName}` (`{formFileBytes.Length:n0}` bytes)");
			}
			catch (Exception ex)
			{
				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: Fatal error: `{ex.Message}`");
			}
		}
	}
}
