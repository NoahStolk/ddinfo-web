using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Entities
{
	public class ResponseLog
	{
		[Key]
		public int Id { get; set; }

		[StringLength(63)]
		public string Path { get; set; } = null!;

		public int ResponseTimeMicroseconds { get; set; }

		public DateTime DateTime { get; set; }
	}
}
