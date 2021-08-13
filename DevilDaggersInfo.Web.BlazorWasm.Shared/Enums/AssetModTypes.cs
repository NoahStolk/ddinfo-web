using System;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Enums
{
	[Flags]
	public enum AssetModTypes
	{
		None = 0,
		Audio = 1,
		Texture = 2,
		Model = 4,
		Shader = 8,
	}
}
