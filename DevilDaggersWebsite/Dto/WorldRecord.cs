using System;

namespace DevilDaggersWebsite.Dto
{
	public class WorldRecord
	{
		public WorldRecord(DateTime dateTime, Entry entry)
		{
			DateTime = dateTime;
			Entry = entry;
		}

		public DateTime DateTime { get; set; }
		public Entry Entry { get; set; }
	}
}