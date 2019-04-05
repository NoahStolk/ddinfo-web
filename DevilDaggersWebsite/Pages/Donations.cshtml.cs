using CoreBase.Services;
using DevilDaggersWebsite.Models.User;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class DonationsModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public List<Donator> Donators { get; set; }

		public DonationsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		//public async Task OnGetAsync()
		public void OnGet()
		{
			Donators = UserUtils.GetDonators(_commonObjects)
				.OrderByDescending(d => d.Amount)
				.ThenBy(d => d.CurrencySymbol, new CurrencyComparer())
				.ThenBy(d => d.Username)
				.ToList();

			//foreach (Donator donator in Donators)
			//{
			//	Entry entry = await Hasmodai.GetUserByID(donator.ID);
			//	donator.Username = entry.Username;
			//}
		}
	}
}