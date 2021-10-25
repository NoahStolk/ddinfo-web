using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Entities
{
	public class Marker
	{
		[Key]
		[StringLength(64)]
		public string Name { get; set; } = null!;

		public long Value { get; set; }
	}
}
