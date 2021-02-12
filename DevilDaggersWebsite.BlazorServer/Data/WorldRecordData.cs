using DevilDaggersWebsite.Dto;
using System;

namespace DevilDaggersWebsite.BlazorServer.Data
{
	public class WorldRecordData
	{
		public WorldRecordData(string username, float time, TimeSpan lasted)
		{
			Username = username;
			Time = time;
			Lasted = lasted;
		}

		public string Username { get; }
		public float Time { get; }
		public TimeSpan Lasted { get; }

		public static WorldRecordData FromWorldRecord(WorldRecord wr, TimeSpan timeLasted)
			=> new(wr.Entry.Username, wr.Entry.Time / 10000f, timeLasted);
	}
}
