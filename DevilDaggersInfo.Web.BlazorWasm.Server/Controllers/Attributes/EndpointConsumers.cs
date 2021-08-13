using System;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Attributes
{
	[Flags]
	public enum EndpointConsumers
	{
		None = 0,
		Ddse = 1,
		Ddcl = 2,
		Ddae = 4,
		Website = 8,
	}
}
