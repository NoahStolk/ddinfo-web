using Microsoft.EntityFrameworkCore;

namespace DevilDaggersWebsite.BlazorWasm.Server.Entities
{
	[Keyless]
	public class InformationSchemaTable
	{
		public string? Table { get; set; }

		public int DataSize { get; set; }

		public int IndexSize { get; set; }

		public int AverageRowLength { get; set; }

		public int TableRows { get; set; }
	}
}
