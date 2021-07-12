﻿using DevilDaggersWebsite.BlazorWasm.Shared.Enums;

namespace DevilDaggersWebsite.Dto
{
	public class ModBinaryPublic
	{
		public string Name { get; init; } = null!;
		public long Size { get; init; }
		public ModBinaryType ModBinaryType { get; init; }
	}
}
