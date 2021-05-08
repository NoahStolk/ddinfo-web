using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Exceptions;
using DevilDaggersWebsite.Razor.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class UploadFileModel : PageModel
	{
		public const int MaxFileSize = 256 * 1024 * 1024;
		public const int MaxFileNameLength = 80;

		public const long MaxHostingSpace = 5L * 1024 * 1024 * 1024;

		private readonly IWebHostEnvironment _env;

		public UploadFileModel(IWebHostEnvironment env)
		{
			_env = env;
		}

		[BindProperty]
		public IFormFile? FormFile { get; set; }

		public async Task OnPostAsync()
		{
			string? filePath = null;

			string failedAttemptMessage = $":x: Failed attempt from `{this.GetIdentity()}` to upload new ASSETMOD file";

			string modsDirectory = Path.Combine(_env.WebRootPath, "mods");

			try
			{
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

				DirectoryInfo di = new(modsDirectory);
				long usedSpace = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
				if (FormFile.Length + usedSpace > MaxHostingSpace)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: This file is {FormFile.Length:n0} bytes in size, but only {MaxHostingSpace - usedSpace:n0} bytes of free space is available.");
					return;
				}

				if (FormFile.FileName.Length > MaxFileNameLength)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File name too long (`{FormFile.FileName.Length}` / max `{MaxFileNameLength}` characters).");
					return;
				}

				if (!FormFile.FileName.EndsWith(".zip"))
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File name must have the `.zip` extension.");
					return;
				}

				filePath = Path.Combine(modsDirectory, FormFile.FileName);
				if (Io.File.Exists(filePath))
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File `{FormFile.FileName}` already exists.");
					return;
				}

				byte[] formFileBytes = new byte[FormFile.Length];
				using (MemoryStream ms = new())
				{
					FormFile.CopyTo(ms);
					formFileBytes = ms.ToArray();
				}

				List<ModBinaryCacheData> archive = ModArchiveCache.Instance.GetArchiveDataByBytes(_env, FormFile.FileName, formFileBytes).Binaries;
				if (archive.Count == 0)
					throw new InvalidModBinaryException($"File `{FormFile.FileName}` does not contain any binaries.");

				foreach (ModBinaryCacheData binary in archive)
				{
					if (binary.Chunks.Count == 0)
						throw new InvalidModBinaryException($"Binary `{binary.Name}` does not contain any assets.");
				}

				Io.File.WriteAllBytes(filePath, formFileBytes);
				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $":white_check_mark: `{this.GetIdentity()}` uploaded new ASSETMOD file :file_folder: `{FormFile.FileName}` (`{formFileBytes.Length:n0}` bytes)");
			}
			catch (InvalidModBinaryException ex)
			{
				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: A binary file inside the file `{FormFile?.FileName}` is invalid. {ex.Message}");
			}
			catch (Exception ex)
			{
				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: Fatal error: `{ex.Message}`");
			}
		}
	}
}
