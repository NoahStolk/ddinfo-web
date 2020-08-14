using DevilDaggersWebsite.Code.Users;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class DonationsModel : PageModel
	{
		public DonationsModel(IWebHostEnvironment env)
		{
			Users = UserUtils.GetUserObjects<User>(env);
			Donations = UserUtils.GetUserObjects<Donation>(env);

			foreach (Donation donation in Donations.Where(d => !d.IsRefunded))
			{
				if (!DonatorsWithReceivedEuroAmounts.ContainsKey(donation.DonatorId))
					DonatorsWithReceivedEuroAmounts.Add(donation.DonatorId, donation.ConvertedEuroCentsReceived);
				else
					DonatorsWithReceivedEuroAmounts[donation.DonatorId] += donation.ConvertedEuroCentsReceived;
			}
		}

		public List<User> Users { get; }
		public List<Donation> Donations { get; }

		public Dictionary<int, int> DonatorsWithReceivedEuroAmounts { get; } = new Dictionary<int, int>();
	}
}