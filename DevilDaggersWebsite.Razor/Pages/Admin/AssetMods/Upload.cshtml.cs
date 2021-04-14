using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Exceptions;
using DevilDaggersWebsite.Razor.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class UploadModel : PageModel
	{
		private readonly IWebHostEnvironment _env;

		public UploadModel(IWebHostEnvironment env)
		{
			_env = env;
		}

		[BindProperty]
		public IFormFile? FormFile { get; set; }

		public async Task OnPostAsync()
		{
			string failedAttemptMessage = $"Failed attempt from `{this.GetIdentity()}` to upload new ASSETMOD file";

			try
			{
				const int maxFileSize = 64 * 1024 * 1024;
				const int maxFileNameLength = 80;

				if (FormFile == null)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: No file.");
					return;
				}

				if (FormFile.Length > maxFileSize)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File too large (`{FormFile.Length}` / max `{maxFileSize}` bytes).");
					return;
				}

				if (FormFile.FileName.Length > maxFileNameLength)
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File name too long (`{FormFile.FileName.Length}` / max `{maxFileNameLength}` characters).");
					return;
				}

				if (!FormFile.FileName.EndsWith(".zip"))
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File name must have the '.zip' extension.");
					return;
				}

				string filePath = Path.Combine(_env.WebRootPath, "mods", FormFile.FileName);
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

				using FileStream fs = new(filePath, FileMode.Open);
				using ZipArchive archive = new(fs);
				foreach (ZipArchiveEntry entry in archive.Entries)
				{
					byte[] extractedContents = new byte[entry.Length];

					using Stream stream = entry.Open();
					stream.Read(extractedContents, 0, extractedContents.Length);

					ModData modData = ModData.CreateFromFile(entry.Name, extractedContents);
					if (modData.ModAssetData.Count == 0)
						throw new InvalidModBinaryException($"File '{entry.Name}' does not contain any assets.");
				}

				Io.File.WriteAllBytes(filePath, formFileBytes);
				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"`{this.GetIdentity()}` uploaded new ASSETMOD file `{FormFile.FileName}`");
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
