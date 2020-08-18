using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Code.Leaderboards.History;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace DevilDaggersWebsite.Code.DataTransferObjects
{
	public class Entry
	{
		[CompletionProperty]
		public int Rank { get; set; }

		[CompletionProperty]
		public int Id { get; set; }

		[CompletionProperty]
		public string Username { get; set; }

		[CompletionProperty]
		public int Time { get; set; }

		[CompletionProperty]
		public int Kills { get; set; }

		[CompletionProperty]
		public int Gems { get; set; }

		[CompletionProperty]
		public int DeathType { get; set; }

		[CompletionProperty]
		public int DaggersHit { get; set; }

		[CompletionProperty]
		public int DaggersFired { get; set; }

		[CompletionProperty]
		public ulong TimeTotal { get; set; }

		[CompletionProperty]
		public ulong KillsTotal { get; set; }

		[CompletionProperty]
		public ulong GemsTotal { get; set; }

		[CompletionProperty]
		public ulong DeathsTotal { get; set; }

		[CompletionProperty]
		public ulong DaggersHitTotal { get; set; }

		[CompletionProperty]
		public ulong DaggersFiredTotal { get; set; }

		[JsonIgnore]
		public double Accuracy => DaggersFired == 0 ? 0 : DaggersHit / (double)DaggersFired;

		[JsonIgnore]
		public double AccuracyTotal => DaggersFiredTotal == 0 ? 0 : DaggersHitTotal / (double)DaggersFiredTotal;

		[JsonIgnore]
		public Completion Completion { get; } = new Completion();

		public Completion GetCompletion()
		{
			if (Completion.Initialized)
				return Completion;

			foreach (PropertyInfo info in GetType().GetProperties())
			{
				object value = info.GetValue(this);
				if (value == null)
					continue;

				string valueString = value.ToString();
				if (string.IsNullOrEmpty(valueString))
					continue;

				if (!Attribute.IsDefined(info, typeof(CompletionPropertyAttribute)))
					continue;

				if (info.Name == nameof(DeathType) && valueString == "-1"
				 || info.Name != nameof(DeathType) && valueString == ReflectionUtils.GetDefaultValue(value.GetType()).ToString())
					Completion.CompletionEntries[info.Name] = CompletionEntry.Missing;
				else
					Completion.CompletionEntries[info.Name] = CompletionEntry.Complete;
			}

			Completion.Initialized = true;
			return Completion;
		}

		public bool IsBlankName()
			=> Id == 999999 || Id == 9999999;
	}
}