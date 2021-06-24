using DevilDaggersCore.Spawnsets;
using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.Razor.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.Razor.Pages.Admin.SpawnsetFiles
{
	public class UploadFileModel : AbstractAdminPageModel
	{
		public const int MaxFileSize = 70 * 1024;
		public const int MaxFileNameLength = 80;

		private readonly IWebHostEnvironment _env;

		public UploadFileModel(IWebHostEnvironment env)
		{
			_env = env;
		}

		[BindProperty]
		public IFormFile? FormFile { get; set; }

		public async Task OnPostAsync()
		{
			string failedAttemptMessage = $":x: Failed attempt from `{GetIdentity()}` to upload new SPAWNSET file";

			try
			{
				if (FormFile == null)
				{
					await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, $"{failedAttemptMessage}: No file.");
					return;
				}

				if (FormFile.Length > MaxFileSize)
				{
					await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, $"{failedAttemptMessage}: File too large (`{FormFile.Length:n0}` / max `{MaxFileSize:n0}` bytes).");
					return;
				}

				if (FormFile.FileName.Length > MaxFileNameLength)
				{
					await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, $"{failedAttemptMessage}: File name too long (`{FormFile.FileName.Length}` / max `{MaxFileNameLength}` characters).");
					return;
				}

				string filePath = Io.Path.Combine(_env.WebRootPath, "spawnsets", FormFile.FileName);
				if (Io.File.Exists(filePath))
				{
					await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, $"{failedAttemptMessage}: File `{FormFile.FileName}` already exists.");
					return;
				}

				byte[] formFileBytes = new byte[FormFile.Length];
				using (Io.MemoryStream ms = new())
				{
					FormFile.CopyTo(ms);
					formFileBytes = ms.ToArray();
				}

				if (!Spawnset.TryParse(formFileBytes, out _))
				{
					await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, $"`{failedAttemptMessage}: File could not be parsed to a proper survival file.`");
					return;
				}

				byte[] spawnsetHash = MD5.HashData(formFileBytes);
				SpawnsetHashCacheData? existingSpawnset = await SpawnsetHashCache.Instance.GetSpawnset(_env, spawnsetHash);
				if (existingSpawnset != null)
				{
					await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, $"`{failedAttemptMessage}: Spawnset is exactly the same as an already existing spawnset named '{existingSpawnset.Name}'.`");
					return;
				}

				Io.File.WriteAllBytes(filePath, formFileBytes);
				await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, $":white_check_mark: `{GetIdentity()}` uploaded new SPAWNSET file :file_folder: `{FormFile.FileName}` (`{formFileBytes.Length:n0}` bytes).");
			}
			catch (Exception ex)
			{
				await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, $"{failedAttemptMessage}: Fatal error: `{ex.Message}`");
			}
		}
	}
}
