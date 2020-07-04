using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Code.Donations
{
	public static class DonationList
	{
		public static Donation[] Donations = new Donation[]
		{
			new Donation(137044, 100, Currency.Eur, 100, new DateTime(2018, 2, 19)),
			new Donation(105315, 1000, Currency.Eur, 1000, new DateTime(2018, 2, 19)),
			new Donation(105315, 1, Currency.Eur, 1, new DateTime(2018, 2, 20)),
			new Donation(94857, 77, Currency.Eur, 39, new DateTime(2018, 2, 19)),
			new Donation(142939, 1200, Currency.Eur, 1200, new DateTime(2018, 2, 20)),
			new Donation(118832, 100, Currency.Eur, 100, new DateTime(2018, 2, 20)),
			new Donation(118832, 150, Currency.Eur, 150, new DateTime(2018, 9, 5)),
			new Donation(118832, 250, Currency.Eur, 250, new DateTime(2018, 10, 22)),
			new Donation(118832, 1, Currency.Usd, 1, new DateTime(2018, 12, 9)),
			new Donation(118832, 9, Currency.Eur, 0, new DateTime(2019, 1, 27)),
			new Donation(118832, 2, Currency.Eur, 0, new DateTime(2019, 1, 27)),
			new Donation(118832, 154, Currency.Eur, 114, new DateTime(2019, 1, 28)),
			new Donation(113530, 500, Currency.Usd, 450, new DateTime(2018, 2, 28)), // TODO: Figure out converted amount.
			new Donation(113530, 500, Currency.Usd, 450, new DateTime(2018, 9, 4)), // TODO: Figure out converted amount.
			new Donation(113530, 500, Currency.Usd, 450, new DateTime(2018, 12, 8)), // TODO: Figure out converted amount.
			new Donation(148518, 500, Currency.Usd, 450, new DateTime(2018, 3, 27)), // TODO: Figure out converted amount.
			new Donation(148951, 750, Currency.Eur, 750, new DateTime(2018, 6, 24)),
			new Donation(148951, 1, Currency.Eur, 1, new DateTime(2018, 6, 24)),
			new Donation(148951, 749, Currency.Eur, 749, new DateTime(2018, 10, 15)),
			new Donation(172395, 500, Currency.Usd, 450, new DateTime(2018, 9, 21)), // TODO: Figure out converted amount.
			new Donation(115431, 500, Currency.Gbp, 559, new DateTime(2018, 9, 21)),
			new Donation(6760, 1000, Currency.Eur, 1000, new DateTime(2018, 9, 28)),
			new Donation(134802, 500, Currency.Eur, 500, new DateTime(2018, 10, 15)),
			new Donation(65617, 2500, Currency.Eur, 2500, new DateTime(2018, 10, 22)),
			new Donation(121891, 500, Currency.Eur, 500, new DateTime(2018, 11, 1)),
			new Donation(184178, 100, Currency.Usd, 86, new DateTime(2018, 12, 19)),
			new Donation(106722, 11, Currency.Eur, 11, new DateTime(2019, 1, 4), true),
			new Donation(106722, 502, Currency.Eur, 502, new DateTime(2019, 1, 4)),
			new Donation(106722, 9, Currency.Eur, 9, new DateTime(2019, 1, 27)),
			new Donation(106722, 2, Currency.Eur, 2, new DateTime(2019, 1, 27)),
			new Donation(106722, 154, Currency.Eur, 154, new DateTime(2019, 1, 29)),
			new Donation(180168, 1, Currency.Usd, 0, new DateTime(2019, 1, 21)),
			new Donation(86805, 6666, Currency.Aud, 4079, new DateTime(2019, 1, 21)),
			new Donation(21059, 2000, Currency.Eur, 2000, new DateTime(2019, 1, 22)),
			new Donation(111007, 1000, Currency.Eur, 1000, new DateTime(2019, 2, 17)),
			new Donation(146931, 2500, Currency.Usd, 2100, new DateTime(2019, 6, 6)), // TODO: Figure out converted amount.
			new Donation(146931, 2500, Currency.Usd, 2100, new DateTime(2019, 9, 11)), // TODO: Figure out converted amount.
			new Donation(134740, 250, Currency.Usd, 217, new DateTime(2019, 8, 12)),
			new Donation(171188, 1000, Currency.Gbp, 1066, new DateTime(2019, 8, 12)),
			new Donation(116299, 2000, Currency.Eur, 2000, new DateTime(2019, 10, 14)),
			new Donation(109193, 1000, Currency.Gbp, 1122, new DateTime(2019, 11, 2)),
			new Donation(197276, 40, Currency.Eur, 40, new DateTime(2019, 12, 28)),
			new Donation(531, 1500, Currency.Eur, 1500, new DateTime(2020, 1, 3)),
			new Donation(1000000, 666, Currency.Gbp, 728, new DateTime(2020, 3, 12)), // TODO: Figure out ID.
			new Donation(118832, 1634, Currency.Eur, 1634, new DateTime(2020, 5, 1)),
			new Donation(111007, 1000, Currency.Eur, 1000, new DateTime(2020, 5, 2)),
			new Donation(1000001, 500, Currency.Gbp, 528, new DateTime(2020, 7, 1)) // TODO: Figure out ID.
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
			new Donator(1000000, "Chung02"),
			new Donator(1000001, "james ☆")
		};

		public static Dictionary<int, int> DonatorsWithReceivedEuroAmounts = new Dictionary<int, int>();

		static DonationList()
		{
			foreach (Donation donation in Donations.Where(d => !d.IsRefunded))
			{
				if (!DonatorsWithReceivedEuroAmounts.ContainsKey(donation.DonatorId))
					DonatorsWithReceivedEuroAmounts.Add(donation.DonatorId, donation.ConvertedEuroCentsReceived);
				else
					DonatorsWithReceivedEuroAmounts[donation.DonatorId] += donation.ConvertedEuroCentsReceived;
			}
		}
	}
}