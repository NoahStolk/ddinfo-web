﻿using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.AssetMods
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public EditModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
		public AssetMod AssetMod { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			AssetMod = await context.AssetMods.FirstOrDefaultAsync(m => m.Id == id);

			if (AssetMod == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			context.Attach(AssetMod).State = EntityState.Modified;

			try
			{
				await context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AssetModExists(AssetMod.Id))
					return NotFound();
				else
					throw;
			}

			return RedirectToPage("./Index");
		}

		private bool AssetModExists(int id) => context.AssetMods.Any(e => e.Id == id);
	}
}