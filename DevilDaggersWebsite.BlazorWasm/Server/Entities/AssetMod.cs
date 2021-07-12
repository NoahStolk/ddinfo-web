using DevilDaggersWebsite.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Server.Entities
{
	public class AssetMod : IEntity
	{
		[Key]
		public int Id { get; init; }

		[StringLength(64)]
		public string Name { get; set; } = null!;

		public bool IsHidden { get; set; }

		public DateTime LastUpdated { get; set; }

		[StringLength(64)]
		public string? TrailerUrl { get; set; }

		[StringLength(2048)]
		public string? HtmlDescription { get; set; }

		public AssetModTypes AssetModTypes { get; set; }

		[StringLength(128)]
		public string Url { get; set; } = null!;

		public List<PlayerAssetMod> PlayerAssetMods { get; set; } = new();
	}
}
