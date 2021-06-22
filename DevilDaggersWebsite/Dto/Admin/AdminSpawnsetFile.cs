﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminSpawnsetFile : IAdminDto
	{
		[StringLength(64)]
		public string Name { get; init; } = null!;

		public int PlayerId { get; init; }

		public int? MaxDisplayWaves { get; init; }

		[StringLength(2048)]
		public string? HtmlDescription { get; init; }

		public DateTime LastUpdated { get; set; } // Use set to default to UtcNow.

		public bool IsPractice { get; init; }

		public Dictionary<string, string> Log()
		{
			Dictionary<string, string> dictionary = new();
			dictionary.Add(nameof(Name), Name);
			dictionary.Add(nameof(PlayerId), PlayerId.ToString());
			dictionary.Add(nameof(MaxDisplayWaves), MaxDisplayWaves.ToString() ?? string.Empty);
			dictionary.Add(nameof(HtmlDescription), HtmlDescription ?? string.Empty);
			dictionary.Add(nameof(LastUpdated), LastUpdated.ToString("dd MMM yyyy"));
			dictionary.Add(nameof(IsPractice), IsPractice.ToString());
			return dictionary;
		}
	}
}
