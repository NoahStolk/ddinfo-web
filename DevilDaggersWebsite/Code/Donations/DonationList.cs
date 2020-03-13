using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Donations
{
	public static class DonationList
	{
		public static Dictionary<string, char> CurrencyChars = new Dictionary<string, char>
		{
			{ "EUR", '€' },
			{ "USD", '$' },
			{ "AUD", '$' },
			{ "GBP", '£' }
		};

		public static Donation[] Donations = new Donation[]
		{
			new Donation(137044, 100, "EUR", 100, new DateTime()), // TODO: Get date.
			new Donation(105315, 1000, "EUR", 1000, new DateTime()), // TODO: Get date.
			new Donation(105315, 1, "EUR", 1, new DateTime(2018, 2, 20)),
			new Donation(94857, 77, "EUR", 39, new DateTime()), // TODO: Get date.
			new Donation(142939, 1200, "EUR", 1200, new DateTime()), // TODO: Get date.
			new Donation(118832, 100, "EUR", 100, new DateTime(2018, 2, 20)),
			new Donation(118832, 150, "EUR", 150, new DateTime(2018, 9, 5)),
			new Donation(118832, 250, "EUR", 250, new DateTime(2018, 10, 22)),
			new Donation(118832, 1, "USD", 1, new DateTime(2018, 12, 9)),
			new Donation(118832, 9, "EUR", 0, new DateTime(2019, 1, 27)),
			new Donation(118832, 2, "EUR", 0, new DateTime(2019, 1, 27)),
			new Donation(118832, 154, "EUR", 114, new DateTime(2019, 1, 28)),
			new Donation(113530, 500, "USD", 450, new DateTime(2018, 2, 28)), // TODO: Figure out converted amount.
			new Donation(113530, 500, "USD", 450, new DateTime(2018, 9, 4)), // TODO: Figure out converted amount.
			new Donation(113530, 500, "USD", 450, new DateTime(2018, 12, 8)), // TODO: Figure out converted amount.
			new Donation(148518, 500, "USD", 450, new DateTime(2018, 3, 27)), // TODO: Figure out converted amount.
			new Donation(148951, 750, "EUR", 750, new DateTime(2018, 6, 24)),
			new Donation(148951, 1, "EUR", 1, new DateTime(2018, 6, 24)),
			new Donation(148951, 749, "EUR", 749, new DateTime(2018, 10, 15)),
			new Donation(172395, 500, "USD", 450, new DateTime(2018, 9, 21)), // TODO: Figure out converted amount.
			new Donation(115431, 500, "GBP", 559, new DateTime(2018, 9, 21)),
			new Donation(6760, 1000, "EUR", 1000, new DateTime(2018, 9, 28)),
			new Donation(134802, 500, "EUR", 500, new DateTime(2018, 10, 15)),
			new Donation(65617, 2500, "EUR", 2500, new DateTime(2018, 10, 22)),
			new Donation(121891, 500, "EUR", 500, new DateTime(2018, 11, 1)),
			new Donation(184178, 100, "USD", 86, new DateTime(2018, 12, 19)),
			//new Donation(106722, 11, "EUR", 11, new DateTime(2019, 1, 4)), // Refunded.
			new Donation(106722, 502, "EUR", 502, new DateTime(2019, 1, 4)),
			new Donation(106722, 9, "EUR", 9, new DateTime(2019, 1, 27)),
			new Donation(106722, 2, "EUR", 2, new DateTime(2019, 1, 27)),
			new Donation(106722, 154, "EUR", 154, new DateTime(2019, 1, 29)),
			new Donation(180168, 1, "USD", 0, new DateTime(2019, 1, 21)),
			new Donation(86805, 6666, "AUD", 4079, new DateTime(2019, 1, 21)),
			new Donation(21059, 2000, "EUR", 2000, new DateTime(2019, 1, 22)),
			new Donation(111007, 1000, "EUR", 1000, new DateTime(2019, 2, 17)),
			new Donation(146931, 2500, "USD", 2100, new DateTime(2019, 6, 6)), // TODO: Figure out converted amount.
			new Donation(146931, 2500, "USD", 2100, new DateTime(2019, 9, 11)), // TODO: Figure out converted amount.
			new Donation(134740, 250, "USD", 217, new DateTime(2019, 8, 12)),
			new Donation(171188, 1000, "GBP", 1066, new DateTime(2019, 8, 12)),
			new Donation(116299, 2000, "EUR", 2000, new DateTime(2019, 10, 14)),
			new Donation(109193, 1000, "GBP", 1122, new DateTime(2019, 11, 2)),
			new Donation(197276, 40, "EUR", 40, new DateTime(2019, 12, 28)),
			new Donation(531, 1500, "EUR", 1500, new DateTime(2020, 1, 3)),
			new Donation(0, 666, "GBP", 728, new DateTime(2020, 3, 12)), // TODO: Figure out ID.
		};

		public static Donator[] Donators = new Donator[]
		{
			new Donator(137044, "DJDoomz"),
			new Donator(105315, "LukeNukem"),
			new Donator(94857, "curry"),
			new Donator(142939, "LocoCaesar_IV"),
			new Donator(118832, "Chupacabra"),
			new Donator(113530, "Zirtonic"),
			new Donator(148518, "Dillon"),
			new Donator(148951, "Stop."),
			new Donator(172395, "Tileä"),
			new Donator(115431, "Pritster"),
			new Donator(6760, "TSTAB"),
			new Donator(134802, "gLad"),
			new Donator(65617, "pagedMov"),
			new Donator(121891, "xamide"),
			new Donator(184178, "Green"),
			new Donator(106722, "pocket", true),
			new Donator(180168, "Perpetucake"),
			new Donator(86805, "Ravenholmzombies"),
			new Donator(21059, "ThePassiveDada"),
			new Donator(111007, "Nito"),
			new Donator(146931, "Duncdaddi"),
			new Donator(134740, "♡MaffyTaffy♡"),
			new Donator(171188, "general223"),
			new Donator(116299, "Bones"),
			new Donator(109193, "Eyther"),
			new Donator(197276, "metalifestorm"),
			new Donator(531, "GaryBanderas"),
			new Donator(0, "Chung02")
		};

		public static Dictionary<int, int> DonatorsWithReceivedEuroAmounts = new Dictionary<int, int>();

		static DonationList()
		{
			foreach (Donation donation in Donations)
			{
				if (!DonatorsWithReceivedEuroAmounts.ContainsKey(donation.DonatorId))
					DonatorsWithReceivedEuroAmounts.Add(donation.DonatorId, donation.ConvertedEuroCentsReceived);
				else
					DonatorsWithReceivedEuroAmounts[donation.DonatorId] += donation.ConvertedEuroCentsReceived;
			}
		}
	}
}