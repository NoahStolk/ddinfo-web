﻿using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Exceptions;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class UploadFileModel : AbstractAdminPageModel
	{
		public const int MaxFileSize = 256 * 1024 * 1024;
		public const int MaxFileNameLength = 80;

		public const long MaxHostingSpace = 5L * 1024 * 1024 * 1024;

		private readonly IWebHostEnvironment _environment;
		private readonly DiscordLogger _discordLogger;
		private readonly ModArchiveCache _modArchiveCache;

		public UploadFileModel(IWebHostEnvironment environment, DiscordLogger discordLogger, ModArchiveCache modArchiveCache)
		{
			_environment = environment;
			_discordLogger = discordLogger;
			_modArchiveCache = modArchiveCache;
		}

		[BindProperty]
		public IFormFile? FormFile { get; set; }

		public async Task OnPostAsync()
		{
			string? filePath = null;

			string failedAttemptMessage = $":x: Failed attempt from `{GetIdentity()}` to upload new ASSETMOD file";

			string modsDirectory = Path.Combine(_environment.WebRootPath, "mods");

			try
			{
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

				DirectoryInfo di = new(modsDirectory);
				long usedSpace = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
				if (FormFile.Length + usedSpace > MaxHostingSpace)
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: This file is {FormFile.Length:n0} bytes in size, but only {MaxHostingSpace - usedSpace:n0} bytes of free space is available.");
					return;
				}

				if (FormFile.FileName.Length > MaxFileNameLength)
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: File name too long (`{FormFile.FileName.Length}` / max `{MaxFileNameLength}` characters).");
					return;
				}

				if (!FormFile.FileName.EndsWith(".zip"))
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: File name must have the `.zip` extension.");
					return;
				}

				filePath = Path.Combine(modsDirectory, FormFile.FileName);
				if (Io.File.Exists(filePath))
				{
					await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: File `{FormFile.FileName}` already exists.");
					return;
				}

				byte[] formFileBytes = new byte[FormFile.Length];
				using (MemoryStream ms = new())
				{
					FormFile.CopyTo(ms);
					formFileBytes = ms.ToArray();
				}

				List<ModBinaryCacheData> archive = _modArchiveCache.GetArchiveDataByBytes(Path.GetFileNameWithoutExtension(FormFile.FileName), formFileBytes).Binaries;
				if (archive.Count == 0)
					throw new InvalidModBinaryException($"File `{FormFile.FileName}` does not contain any binaries.");

				string archiveNameWithoutExtension = Path.GetFileNameWithoutExtension(FormFile.FileName);

				foreach (ModBinaryCacheData binary in archive)
				{
					if (binary.Chunks.Count == 0)
						throw new InvalidModBinaryException($"Binary `{binary.Name}` does not contain any assets.");

					string expectedPrefix = binary.ModBinaryType switch
					{
						ModBinaryType.Audio => $"audio-{archiveNameWithoutExtension}-",
						ModBinaryType.Dd => $"dd-{archiveNameWithoutExtension}-",
						_ => throw new InvalidModBinaryException($"Binary `{binary.Name}` is a `{binary.ModBinaryType}` mod which is not allowed."),
					};

					if (!binary.Name.StartsWith(expectedPrefix))
						throw new InvalidModBinaryException($"Name of binary `{binary.Name}` must start with `{expectedPrefix}`.");

					if (binary.Name.Length == expectedPrefix.Length)
						throw new InvalidModBinaryException($"Name of binary `{binary.Name}` must not be equal to `{expectedPrefix}`.");
				}

				Io.File.WriteAllBytes(filePath, formFileBytes);
				await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $":white_check_mark: `{GetIdentity()}` uploaded new ASSETMOD file :file_folder: `{FormFile.FileName}` (`{formFileBytes.Length:n0}` bytes)");
			}
			catch (InvalidModBinaryException ex)
			{
				await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: A binary file inside the file `{FormFile?.FileName}` is invalid. {ex.Message}");
			}
			catch (Exception ex)
			{
				await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: Fatal error: `{ex.Message}`");
			}
		}
	}
}
