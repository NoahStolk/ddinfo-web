using DevilDaggersCore.Spawnsets;
using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Razor.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.Razor.Pages.Admin.SpawnsetFiles
{
	public class UploadFileModel : PageModel
	{
		private readonly IWebHostEnvironment _env;

		public UploadFileModel(IWebHostEnvironment env)
		{
			_env = env;
		}

		[BindProperty]
		public IFormFile? FormFile { get; set; }

		public async Task OnPostAsync()
		{
			string failedAttemptMessage = $"Failed attempt from `{this.GetIdentity()}` to upload new SPAWNSET file";

			try
			{
				const int maxFileSize = 70 * 1024;
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

				string filePath = Io.Path.Combine(_env.WebRootPath, "spawnsets", FormFile.FileName);
				if (Io.File.Exists(filePath))
				{
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File `{FormFile.FileName}` already exists.");
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
					await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"`{failedAttemptMessage}: File could not be parsed to a proper survival file.`");
					return;
				}

				Io.File.WriteAllBytes(filePath, formFileBytes);
				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"`{this.GetIdentity()}` uploaded new SPAWNSET file `{FormFile.FileName}`");
			}
			catch (Exception ex)
			{
				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: Fatal error: `{ex.Message}`");
			}
		}
	}
}
